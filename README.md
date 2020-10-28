# azure-cosmosdb-cost-calculator

Calculate CosmosDB costs using DotNet Core code and a "Cucumber-like" specification syntax.

You **do not** have to modify the code; just create specification file(s)
for your particular use-cases.

## Getting Started

This calculator assumes that you have [git](https://git-scm.com/) and 
[DotNet Core 3.1](https://dotnet.microsoft.com/download/dotnet-core) 
installed on your system.

If so, then execute the following commands in a Terminal/PowerShell window.

```
$ git clone https://github.com/cjoakim/azure-cosmosdb-cost-calculator.git
$ cd azure-cosmosdb-cost-calculator 
$ cd cosmos_calc
$ dotnet restore
$ dotnet build
```


## Usage - Example 1

First, define your CosmosDB databases/container specifications in a **Cucumber-like** 
text format like the following:

```
Sample Costs Specification #1
This file is passed to Program.cs

container:                   events1
provisioning_type:          standard
replication_type:             single
region_count:                      1
size_in_gb:                        1
max_historical_manual_ru:       1000
max_historical_auto_ru:            0
ru_per_second:                   500
availability_zone:             false
calculate_costs:                true

container:                   events2
provisioning_type:          standard
replication_type:             single
region_count:                      1
size_in_gb:                        1
max_historical_manual_ru:       1000
max_historical_auto_ru:            0
ru_per_second:                   500
availability_zone:              true
calculate_costs:                true

container:              assignment1a
provisioning_type:          standard
replication_type:             single
region_count:                      1
size_in_tb:                        1
max_historical_manual_ru:          0
max_historical_auto_ru:            0
ru_per_second:                200000
availability_zone:             false
calculate_costs:                true

container:              assignment1b
provisioning_type:          standard
replication_type:             single
region_count:                      1
size_in_tb:                      100
max_historical_manual_ru:          0
max_historical_auto_ru:            0
ru_per_second:                200000
availability_zone:             false
calculate_costs:                true
```

Then execute Program.cs, passing the name of the text specification file:

```
$ dotnet run specification1.txt
```

The output is in JSON format, and contains both the specification values
for each container as well as the calculated costs for it.

```
Using input file: specification1.txt
{
  "name": "events1",
  "sizeInGB": 1,
  "provisioningType": "standard",
  "availabilityZone": false,
  "replicationType": "single",
  "regionCount": 1,
  "ruPerSecond": 500,
  "maxHistoricalManualRu": 1000,
  "maxHistoricalAutoRu": 0,
  "calculatedMinRU": 400,
  "calculatedRatePer100RU": 0.008,
  "calculatedRUInHundreds": 5,
  "calculatedRuDollarsPerHour": 0.04,
  "calculatedRuDollarsPerMonth": 29.12,
  "calculatedStoragePerMonth": 0.25,
  "calculatedTotalPerMonth": 29.37
}
{
  "name": "events2",
  "sizeInGB": 1,
  "provisioningType": "standard",
  "availabilityZone": true,
  "replicationType": "single",
  "regionCount": 1,
  "ruPerSecond": 500,
  "maxHistoricalManualRu": 1000,
  "maxHistoricalAutoRu": 0,
  "calculatedMinRU": 400,
  "calculatedRatePer100RU": 0.01,
  "calculatedRUInHundreds": 5,
  "calculatedRuDollarsPerHour": 0.05,
  "calculatedRuDollarsPerMonth": 36.4,
  "calculatedStoragePerMonth": 0.25,
  "calculatedTotalPerMonth": 36.65
}
{
  "name": "assignment1a",
  "sizeInGB": 1024,
  "provisioningType": "standard",
  "availabilityZone": false,
  "replicationType": "single",
  "regionCount": 1,
  "ruPerSecond": 200000,
  "maxHistoricalManualRu": 0,
  "maxHistoricalAutoRu": 0,
  "calculatedMinRU": 10300,
  "calculatedRatePer100RU": 0.008,
  "calculatedRUInHundreds": 2000,
  "calculatedRuDollarsPerHour": 16,
  "calculatedRuDollarsPerMonth": 11648,
  "calculatedStoragePerMonth": 256,
  "calculatedTotalPerMonth": 11904
}
{
  "name": "assignment1b",
  "sizeInGB": 102400,
  "provisioningType": "standard",
  "availabilityZone": false,
  "replicationType": "single",
  "regionCount": 1,
  "ruPerSecond": 200000,
  "maxHistoricalManualRu": 0,
  "maxHistoricalAutoRu": 0,
  "calculatedMinRU": 1024000,
  "calculatedRatePer100RU": 0.008,
  "calculatedRUInHundreds": 2000,
  "calculatedRuDollarsPerHour": 16,
  "calculatedRuDollarsPerMonth": 11648,
  "calculatedStoragePerMonth": 25600,
  "calculatedTotalPerMonth": 37248
}
```

---

## Usage - Example 2

### Specification File

```
Sample Costs Specification #2
This file is passed to Program.cs

container:                 customers
provisioning_type:         autoscale
replication_type:       multi-master
region_count:                      2
size_in_tb:                      6.2
calculate_costs:                true
```

### Execution and Output

```
$ dotnet run specification2.txt

Using input file: specification2.txt
{
  "name": "customers",
  "sizeInGB": 6348.8,
  "provisioningType": "autoscale",
  "availabilityZone": false,
  "replicationType": "multi-master",
  "regionCount": 2,
  "ruPerSecond": 0,
  "maxHistoricalManualRu": 0,
  "maxHistoricalAutoRu": 0,
  "calculatedMinRU": 63500,
  "calculatedRatePer100RU": 0.016,
  "calculatedRUInHundreds": 0,
  "calculatedRuDollarsPerHour": 0,
  "calculatedRuDollarsPerMonth": 0,
  "calculatedStoragePerMonth": 1587.2,
  "calculatedTotalPerMonth": 1587.2
}
```

---

## Specification Syntax used by this Calculator

```
Statement                    Values
---------                    ------
container:                   Your container name or use-case name
provisioning_type:           standard (default) or autoscale
replication_type:            single (default), multi-region, or multi-master
ru_per_second:               The number of RUs in the container
region_count:                The number of regions, defaults to 1
availability_zone:           Boolean, defaults to false
size_in_bytes:               Specify the storage quantity in terms of bytes
size_in_mb:                  Specify the storage quantity in terms of MB
size_in_gb:                  Specify the storage quantity in terms of GB
size_in_tb:                  Specify the storage quantity in terms of TB
max_historical_manual_ru:    Optional
max_historical_auto_ru:      Optional

calculate_min_ru:            boolean, triggers a Minimum RU calculation
calculate_costs:             boolean, triggers a Cost calculation
```

---

## This Project in Visual Studio

Xunit unit tests are included in this project.

<p align="center" width="95%">
  <img src="img/cosmos-calculator-in-visual-studio.png">
</p>

---

## What is Cucumber?

It's a software testing framework that allows you to express tests
in an English-like syntax of your own creation, see: 
[Cucumber](https://en.wikipedia.org/wiki/Cucumber_(software))
