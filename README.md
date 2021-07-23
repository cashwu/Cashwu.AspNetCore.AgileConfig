# Cashwu.AspNetCore.AgileConfig

## 01 execute [init.sql](https://github.com/cashwu/Cashwu.AspNetCore.AgileConfig/blob/master/Cashwu.AspNetCore.AgileConfig/init.sql)

## 02 add db connection in `appsettings`

```
{
  "ConnectionStrings": {
    "AgileConfigConnection": "Data Source=127.0.0.1;Initial Catalog=AgileConfig;Persist Security Info=True;User ID=sa;Password=sa;MultipleActiveResultSets=False"
  }
}
```

## 02 add project setup in `Program`

- `ConnectionStringName` is appsettings `ConnectionStrings` key
- `PollingInterval` is interval second to query db

```
 Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration((_, builder) =>
     {
         builder.AddEntityFrameworkValues(options =>
         {
             options.ConnectionStringName = "AgileConfigConnection";
             options.PollingInterval = 10;
         });
     });
```
