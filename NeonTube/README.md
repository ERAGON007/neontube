
# Youtube video downloader written in .NET 5
## TODO List
- Save sent videos unique file id in a database for later uses
- Improve `VideoQueueJob`
- Add `help` command
## Running on local
1. First of all you will need to download and install [ngrok](https://ngrok.com/download)
2. Fire up your ngrok using command `ngrok http 8443` (Note: Telegram API only supports ports 443, 80, 88 or 8443) you can put one of them instead of `8443`
3. `ngrok` will give you a temporary domain that's forwarded to your machine running asp.net core, get that link in your ngrok console (your SHOULD use the https one) and set your webhook to that url
4. Now it's time to tell our project to listen on the port you used in `Step 2`, open `Program.cs` and put your port in line `24`
5. Set your bot's token in `appsettings.json` and `appsettings.Development.json`
6. Build and start the bot :)


### How to set Webhook
From ngrok you get an URL to your local server. It’s important to use the `https` one. You can post this url als form-data (key: url, value: https://yoursubdomain.ngrok.io/api/update) to the telegram api.
https://api.telegram.org/botYourBotToken/setWebhook
Be aware of the **bot** prefix in front of your bot token in the URL.
