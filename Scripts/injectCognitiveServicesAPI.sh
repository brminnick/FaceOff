#!/bin/bash

sed -i '' "s/Add Emotion API Key Here/$2/g" "$1"
sed -i '' "s/#error/\/\/#error/g" "$1"
