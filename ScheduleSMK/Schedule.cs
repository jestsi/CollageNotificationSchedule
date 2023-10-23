// YApi QuickType插件生成，具体参考文档:https://plugins.jetbrains.com/plugin/18847-yapi-quicktype/documentation

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Schedule
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("list")]
        public List<List1> List { get; set; }
    }

    public partial class List1
    {
        [JsonProperty("timeEnd")]
        public string TimeEnd { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("auditorium")]
        public string Auditorium { get; set; }

        [JsonProperty("types")]
        public string Types { get; set; }

        [JsonProperty("timeStart")]
        public string TimeStart { get; set; }

        [JsonProperty("subgroup")]
        public long Subgroup { get; set; }

        [JsonProperty("disciplines")]
        public string Disciplines { get; set; }

        [JsonProperty("teachers")]
        public string Teachers { get; set; }

        [JsonProperty("corpus")]
        public string Corpus { get; set; }
    }

    public partial class Schedule
    {
        public static List<Schedule> FromJson(string json) => JsonConvert.DeserializeObject <List<Schedule>>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Schedule self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
