# DbConnection Wrapper [![NuGet](https://img.shields.io/nuget/v/Pustalorc.DbConnectionWrapper.svg)](https://www.nuget.org/packages/Pustalorc.DbConnectionWrapper/)

Library that wraps the ADO.NET [DbConnection](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) class and adds more methods to execute queries whilst using less of the exact same code in a medion/large sized project.

# Installation & Usage

Before you begin, please make sure that your current solution has installed the nuget package of this library.  
You can find this package [here.](https://www.nuget.org/packages/Pustalorc.DbConnectionWrapper)

To use this wrapper, you can see how [MySqlConnectorWrapper](https://github.com/Pustalorc/MySqlConnectorWrapper) does this.
Specifically, one should look at the following files:
- [MySqlConnectionWrapper.cs](https://github.com/Pustalorc/MySqlConnectorWrapper/blob/master/MySqlDatabaseWrapper/Abstractions/MySqlConnectionWrapper.cs)
- [MySqlDataWrapper.cs](https://github.com/Pustalorc/MySqlConnectorWrapper/blob/master/MySqlDatabaseWrapper/Implementations/MySqlDataWrapper.cs)
- [MySqlDataWrapperTests.cs](https://github.com/Pustalorc/MySqlConnectorWrapper/blob/master/MySqlDatabaseWrapper.Tests/MySqlDataWrapperTests.cs)