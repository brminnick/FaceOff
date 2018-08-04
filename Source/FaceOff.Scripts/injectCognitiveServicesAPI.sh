#!/bin/bash

sed -i '' "s/Add Face API Key Here/$2/g" "$1"
sed -i '' "s/#error Face API Key Missing/\/\/#error Face API Key Missing/g" "$1"


sed -i '' "s/Add Face API Base Url Here/$3/g" "$1"
sed -i '' "s/#error Base Url Missing/\/\/#error Base Url Missing/g" "$1"