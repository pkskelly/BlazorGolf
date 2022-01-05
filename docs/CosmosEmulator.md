# Local Development 

## Cosmos DB Emulator

See [Run the emulator on Docker for Linux (Preview)](https://docs.microsoft.com/en-us/azure/cosmos-db/linux-emulator?tabs=ssl-netstd21) for details on running the CosmosDB Emulator in Docker for local development. 

## Local Development

Once the emulator is running, update the BlazorGold.Api appsettings.Development.json file with the ```DefaultConnection``` string to the connection string available at ```https://localhost:8081/_explorer/index.html```
