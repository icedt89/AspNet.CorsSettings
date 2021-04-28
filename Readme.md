# How to configure

Sample settings section for appsettings*.json

```json
{
  "Cors": {
    "Policies": [{
      "IsDefaultPolicy": true,
      "Name": "MyDefaultPolicy",
      "ExposedHeaders": [],
      "Headers": [ "*" ],
      "Methods": [ "*" ],
      "Origins": [ "https://localhost:4200" ],
      "SupportsCredentials": true
    }, {
      "Name": "MyLeastRestrictivePolicy",
      "ExposedHeaders": [],
      "Headers": [ "*" ],
      "Methods": [ "*" ],
      "Origins": [ "*" ],
      "SupportsCredentials": false
    }, {
      "Name": "AnotherSample",
      "ExposedHeaders": [],
      "Headers": [ "Content-Type" ],
      "Methods": [ "GET", "PUT", "PATCH" ],
      "Origins": [ "*" ],
      "SupportsCredentials": false
    }]
  }
}
```