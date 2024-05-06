# JHA-Reddit
JHA Reddit Application

The clientId and secret need to be configured to run the application. The configuration for these can be found under the Console project in the config.json file.

```json
{
  "RedditConfiguration": {
    "BaseTokenUrl": "https://www.reddit.com/",
    "BaseApiUrl": "https://oauth.reddit.com/",
    //# "ClientId": "",
    //# "ClientSecret": "",
    "Feeds": [
      {
        "Name": "hunting",
        "SubReddit": "r/hunting"
      },
      {
        "Name": "funny",
        "SubReddit": "r/funny"
      }
    ]
  }
}
