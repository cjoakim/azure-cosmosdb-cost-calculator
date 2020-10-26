# azure-cosmosdb-cost-calculator

Calculate CosmosDB costs using DotNet Core and a "Cucumber-like" specification syntax

## Usage

First, define your CosmosDB databases/containers in a **Cucumber-like** 
specification text format like the following:

```

```

Then execute Program.cs, passing that text specification file:

```

```

The output is in JSON format, and contains both the specification values
for each container as well as the calculated costs for it.

```

```

[Cucumber](https://en.wikipedia.org/wiki/Cucumber_(software))

---

## This Project in Visual Studio

Xunit unit tests are included in this project.

<p align="center" width="95%">
  <img src="img/cosmos-calculator-in-visual-studio.png">
</p>
