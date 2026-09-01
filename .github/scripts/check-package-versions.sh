#!/usr/bin/env bash
#
# Fails if a packable project changed without a VersionPrefix bump.
#
# publish.yml packs the version straight from the csproj and pushes with
# --skip-duplicate, so a forgotten bump publishes nothing and still reports
# success. This catches it on the PR instead.
#
# Run locally before pushing:  BASE_REF=main .github/scripts/check-package-versions.sh

set -euo pipefail

BASE_REF="${BASE_REF:-main}"

git fetch --no-tags --depth=1 origin "$BASE_REF"
base=$(git rev-parse FETCH_HEAD)

# On a pull_request event HEAD is the merge commit, so a tree diff against the
# base tip is exactly what the PR changes. No merge base needed, which means a
# shallow clone is enough.
changed() {
  local dir=$1

  ! git diff --quiet "$base" HEAD -- "$dir"
}

extract_version() {
  sed -n 's/.*<VersionPrefix>\(.*\)<\/VersionPrefix>.*/\1/p'
}

failed=0

check() {
  local name=$1 csproj=$2
  local old new highest line major minor suggested

  old=$(git show "$base:$csproj" | extract_version)
  new=$(extract_version <"$csproj")

  if [[ -z $new ]]; then
    echo "::error file=$csproj::$name has no <VersionPrefix>" >&2
    failed=1
    return
  fi

  # New packable project on this branch — nothing to bump from.
  if [[ -z $old ]]; then
    echo "OK  $name is new, publishing at $new"
    return
  fi

  highest=$(printf '%s\n%s\n' "$old" "$new" | sort -V | tail -1)
  if [[ $new == "$old" || $new != "$highest" ]]; then
    line=$(grep -n '<VersionPrefix>' "$csproj" | head -1 | cut -d: -f1)
    IFS=. read -r major minor _ <<<"$old"
    suggested="$major.$((minor + 1)).0"
    echo "::error file=$csproj,line=$line::$name changed but its VersionPrefix ($new) is not ahead of $BASE_REF ($old) — bump it (e.g. $suggested)" >&2
    failed=1
    return
  fi

  echo "OK  $name bumped $old -> $new"
}

domain_changed=false
if changed src/Domain; then
  domain_changed=true
fi

# Api.Client has a ProjectReference to Domain, so packing it embeds the current
# Domain version as a dependency. A Domain-only bump leaves the published client
# pointing at a stale Domain package.
client_changed=false
if changed src/Api.Client || [[ $domain_changed == true ]]; then
  client_changed=true
fi

if [[ $domain_changed == true ]]; then
  check Domain src/Domain/Domain.csproj
else
  echo "--  Domain unchanged"
fi

if [[ $client_changed == true ]]; then
  check Api.Client src/Api.Client/Api.Client.csproj
else
  echo "--  Api.Client unchanged"
fi

exit $failed
