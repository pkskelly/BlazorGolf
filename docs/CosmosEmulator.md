# Local Development

## Data Modeling

Part of this project was to learn a bit about using CosmosDB and the associated [Data Modeling](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/modeling-data) changes from SQL databases vs. document databases.  Using CosmosDB is an exploration and will likely have some twists as I figure out what should be stored as a single document, and what should reference links as needed.

> The Emulator is not supported for the M1 chips.

## Cosmos DB Emulator

See [Run the emulator on Docker for Linux (Preview)](https://docs.microsoft.com/en-us/azure/cosmos-db/linux-emulator?tabs=ssl-netstd21) for details on running the CosmosDB Emulator in Docker for local development. 

## Local Development

Once the emulator is running, update the BlazorGold.Api appsettings.Development.json file with the ```DefaultConnection``` string to the connection string available at ```https://localhost:8081/_explorer/index.html```


You may need to reload the certificate for local use as needed.  Follow the instructions. 

