using dev_library.Data;
using dev_library.Data.WoW.Raidbots;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace dev_library.Clients
{
    public class GoogleSheetsClient
    {


        public GoogleSheetsClient()
        {
            SheetsService = GetSheetsService();
        }

        readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        readonly SheetsService SheetsService;

        private SheetsService GetSheetsService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(AppSettings.GoogleSheet.CredentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppSettings.GoogleSheet.SheetName,
            });
        }

        private async Task<List<ItemUpgrade>> ReadEntries()
        {
            var range = $"{AppSettings.GoogleSheet.SheetName}!A:E"; // Assuming headers are in row 1
            var request = SheetsService.Spreadsheets.Values.Get(AppSettings.GoogleSheet.Id, range);
            var response = await request.ExecuteAsync();

            var values = response.Values;
            var entries = new List<ItemUpgrade>();

            if (values != null)
            {
                foreach (var row in values)
                {
                    if (row.Count < 5) continue; // Skip incomplete rows

                    entries.Add(new ItemUpgrade(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), double.Parse(row[4].ToString())));
                }
            }
            return entries;
        }

        private async Task ClearSheet()
        {
            var requestBody = new ClearValuesRequest();
            var request = SheetsService.Spreadsheets.Values.Clear(requestBody, AppSettings.GoogleSheet.Id, $"{AppSettings.GoogleSheet.SheetName}!A:E");
            await request.ExecuteAsync();
        }

        private async Task WriteEntries(List<ItemUpgrade> entries)
        {
            var range = $"{AppSettings.GoogleSheet.SheetName}!A:E";
            var values = new List<IList<object>>();

            foreach (var entry in entries)
            {
                values.Add(new List<object> { entry.PlayerName, entry.Slot, entry.Difficulty, entry.ItemId, entry.DpsGain });
            }

            var requestBody = new ValueRange { Values = values };
            var request = SheetsService.Spreadsheets.Values.Update(requestBody, AppSettings.GoogleSheet.Id, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            await request.ExecuteAsync();
        }

        public async Task<bool> UpdateSheet(List<ItemUpgrade> newEntries)
        {
            // Step 1: Read current data from the sheet
            var sheetData = await ReadEntries();

            // Step 2: Remove existing entries that match "Player Name" and "Difficulty"
            sheetData.RemoveAll(existing =>
                newEntries.Any(newEntry =>
                    existing.PlayerName.ToUpper() == newEntry.PlayerName.ToUpper() &&
                    existing.Difficulty.ToUpper() == newEntry.Difficulty.ToUpper()));

            // Step 3: Append new data
            sheetData.AddRange(newEntries);

            // Step 4: Clear & Update Sheet
            await ClearSheet();
            await WriteEntries(sheetData);

            return true;
        }
    }
}
