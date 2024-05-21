using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Services.TextParserService
{
    public class TextParserAuthenService
    {
        public Dictionary<string, string> ParseTextFrontOfCard(string text)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();

            // Split the text into lines
            string[] lines = text.Split('\n');

            // Define field names and their expected positions
            Dictionary<string, int> fieldPositions = new Dictionary<string, int>()
            {
                { "Số/No:", 0 },
                { "Họ và tên/ Full name:", 1 },
                { "Ngày sinh/ Date of birth:", 2 },
                { "Giới tính/ Sex:", 3 },
                { "Quốc tịch/ Nationality:", 4 },
                { "Quê quán/ Place of origin:", 5 },
                { "Nơi thường trú/ Place of residence:", 6 },
                { "Có giá trị đến:", 7 },
                // Add more fields as needed
            };

            foreach (var kvp in fieldPositions)
            {
                string fieldName = kvp.Key;
                int position = kvp.Value;

                // Ensure the position is within bounds
                if (position >= 0 && position < lines.Length)
                {
                    // Extract the field value from the corresponding line
                    string fieldValue = "";

                    // Continue reading lines until encountering the next field name or reaching the end of lines
                    for (int i = position + 1; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Số/No:")
                            || lines[i].StartsWith("Họ và tên/ Full name:")
                            || lines[i].StartsWith("Ngày sinh/ Date of birth:")
                            || lines[i].StartsWith("Giới tính/ Sex:")
                            || lines[i].StartsWith("Quốc tịch/ Nationality:")
                            || lines[i].StartsWith("Quê quán/ Place of origin:")
                            || lines[i].StartsWith("Nơi thường trú/ Place of residence:")
                            || lines[i].StartsWith("Có giá trị đến:") )
                        {
                            break;
                        }

                        fieldValue += lines[i].Trim() + " ";
                    }

                    // Trim any leading or trailing whitespace
                    fieldValue = fieldValue.Trim();

                    fields.Add(fieldName, fieldValue);
                }
                else
                {
                    // If the position is out of bounds, set the field value to an empty string
                    fields.Add(fieldName, "");
                }
            }

            return fields;
        }

        public Dictionary<string, string> ParseTextBackOfCard(string text)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();

            // Split the text into lines
            string[] lines = text.Split('\n');

            // Define field names and their expected positions
            Dictionary<string, int> fieldPositions = new Dictionary<string, int>()
            {
                { "Ngày, tháng, năm/ Date, month, year:", 0 },
                { " ", 1 },
                // Add more fields as needed
            };

            foreach (var kvp in fieldPositions)
            {
                string fieldName = kvp.Key;
                int position = kvp.Value;

                // Ensure the position is within bounds
                if (position >= 0 && position < lines.Length)
                {
                    // Extract the field value from the corresponding line
                    string fieldValue = "";

                    // Continue reading lines until encountering the next field name or reaching the end of lines
                    for (int i = position + 1; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Ngày, tháng, năm/ Date, month, year:"))
                        {
                            break;
                        }

                        fieldValue += lines[i].Trim() + " ";
                    }

                    // Trim any leading or trailing whitespace
                    fieldValue = fieldValue.Trim();

                    fields.Add(fieldName, fieldValue);
                }
                else
                {
                    // If the position is out of bounds, set the field value to an empty string
                    fields.Add(fieldName, "");
                }
            }

            return fields;
        }
    }
}
