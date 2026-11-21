//The app runs in the browser as WebAssembly. Nothing needs to happen at startup:
//the page calls TreasureApi.Roll (TreasureApi.cs) whenever the Castle Keeper rolls.
CommonUtils.InitRandom();
