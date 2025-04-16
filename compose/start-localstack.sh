#!/bin/bash

export AWS_ENDPOINT_URL=http://localhost:4566
export AWS_REGION=eu-west-2
export AWS_DEFAULT_REGION=eu-west-2
export AWS_ACCESS_KEY_ID=test
export AWS_SECRET_ACCESS_KEY=test

aws --endpoint-url=http://localhost:4566 sns create-topic \
    --name trade_imports_data_upserted

aws --endpoint-url=http://localhost:4566 sqs create-queue \
    --queue-name trade_imports_data_upserted_queue

aws --endpoint-url=http://localhost:4566 sns subscribe \
    --topic-arn arn:aws:sns:eu-west-2:000000000000:trade_imports_data_upserted \
    --protocol sqs \
    --notification-endpoint arn:aws:sqs:eu-west-2:000000000000:trade_imports_data_upserted_queue \
    --attributes '{"RawMessageDelivery": "true"}'

function is_ready() {
    aws --endpoint-url=http://localhost:4566 sns list-topics --query "Topics[?ends_with(TopicArn, ':trade_imports_data_upserted')].TopicArn" || return 1
    return 0
}

while ! is_ready; do
    echo "Waiting until ready"
    sleep 1
done

touch /tmp/ready
