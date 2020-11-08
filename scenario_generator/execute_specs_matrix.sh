#!/bin/bash

# Execute the generated matrix of specifications.
# Chris Joakim, Microsoft, 2020-11-07

dotnet run specs/1-standard-single-1-azone-3gb.txt > 1-standard-single-1-azone-3gb.json
dotnet run specs/2-standard-single-1-azone-300gb.txt > 2-standard-single-1-azone-300gb.json
dotnet run specs/3-standard-single-1-azone-30000gb.txt > 3-standard-single-1-azone-30000gb.json
dotnet run specs/4-standard-single-1-noazone-3gb.txt > 4-standard-single-1-noazone-3gb.json
dotnet run specs/5-standard-single-1-noazone-300gb.txt > 5-standard-single-1-noazone-300gb.json
dotnet run specs/6-standard-single-1-noazone-30000gb.txt > 6-standard-single-1-noazone-30000gb.json
dotnet run specs/7-standard-multi-region-3-azone-3gb.txt > 7-standard-multi-region-3-azone-3gb.json
dotnet run specs/8-standard-multi-region-3-azone-300gb.txt > 8-standard-multi-region-3-azone-300gb.json
dotnet run specs/9-standard-multi-region-3-azone-30000gb.txt > 9-standard-multi-region-3-azone-30000gb.json
dotnet run specs/10-standard-multi-region-3-noazone-3gb.txt > 10-standard-multi-region-3-noazone-3gb.json
dotnet run specs/11-standard-multi-region-3-noazone-300gb.txt > 11-standard-multi-region-3-noazone-300gb.json
dotnet run specs/12-standard-multi-region-3-noazone-30000gb.txt > 12-standard-multi-region-3-noazone-30000gb.json
dotnet run specs/13-standard-multi-master-3-azone-3gb.txt > 13-standard-multi-master-3-azone-3gb.json
dotnet run specs/14-standard-multi-master-3-azone-300gb.txt > 14-standard-multi-master-3-azone-300gb.json
dotnet run specs/15-standard-multi-master-3-azone-30000gb.txt > 15-standard-multi-master-3-azone-30000gb.json
dotnet run specs/16-standard-multi-master-3-noazone-3gb.txt > 16-standard-multi-master-3-noazone-3gb.json
dotnet run specs/17-standard-multi-master-3-noazone-300gb.txt > 17-standard-multi-master-3-noazone-300gb.json
dotnet run specs/18-standard-multi-master-3-noazone-30000gb.txt > 18-standard-multi-master-3-noazone-30000gb.json
dotnet run specs/19-autoscale-single-1-azone-3gb.txt > 19-autoscale-single-1-azone-3gb.json
dotnet run specs/20-autoscale-single-1-azone-300gb.txt > 20-autoscale-single-1-azone-300gb.json
dotnet run specs/21-autoscale-single-1-azone-30000gb.txt > 21-autoscale-single-1-azone-30000gb.json
dotnet run specs/22-autoscale-single-1-noazone-3gb.txt > 22-autoscale-single-1-noazone-3gb.json
dotnet run specs/23-autoscale-single-1-noazone-300gb.txt > 23-autoscale-single-1-noazone-300gb.json
dotnet run specs/24-autoscale-single-1-noazone-30000gb.txt > 24-autoscale-single-1-noazone-30000gb.json
dotnet run specs/25-autoscale-multi-region-3-azone-3gb.txt > 25-autoscale-multi-region-3-azone-3gb.json
dotnet run specs/26-autoscale-multi-region-3-azone-300gb.txt > 26-autoscale-multi-region-3-azone-300gb.json
dotnet run specs/27-autoscale-multi-region-3-azone-30000gb.txt > 27-autoscale-multi-region-3-azone-30000gb.json
dotnet run specs/28-autoscale-multi-region-3-noazone-3gb.txt > 28-autoscale-multi-region-3-noazone-3gb.json
dotnet run specs/29-autoscale-multi-region-3-noazone-300gb.txt > 29-autoscale-multi-region-3-noazone-300gb.json
dotnet run specs/30-autoscale-multi-region-3-noazone-30000gb.txt > 30-autoscale-multi-region-3-noazone-30000gb.json
dotnet run specs/31-autoscale-multi-master-3-azone-3gb.txt > 31-autoscale-multi-master-3-azone-3gb.json
dotnet run specs/32-autoscale-multi-master-3-azone-300gb.txt > 32-autoscale-multi-master-3-azone-300gb.json
dotnet run specs/33-autoscale-multi-master-3-azone-30000gb.txt > 33-autoscale-multi-master-3-azone-30000gb.json
dotnet run specs/34-autoscale-multi-master-3-noazone-3gb.txt > 34-autoscale-multi-master-3-noazone-3gb.json
dotnet run specs/35-autoscale-multi-master-3-noazone-300gb.txt > 35-autoscale-multi-master-3-noazone-300gb.json
dotnet run specs/36-autoscale-multi-master-3-noazone-30000gb.txt > 36-autoscale-multi-master-3-noazone-30000gb.json
