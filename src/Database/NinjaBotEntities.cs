namespace NinjaBotCore.Database
{
    using System;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.SqlServer;
    using Microsoft.Extensions.Configuration;

    public partial class NinjaBotEntities : DbContext
    {
        private IConfigurationRoot _config;

        public virtual DbSet<RlStat> RlStats { get; set; }
        public virtual DbSet<RlUserStat> RlUserStats { get; set; }
        public virtual DbSet<TriviaQuestion> TriviaQuestion { get; set; }
        public virtual DbSet<TriviaQuestionChoice> TriviaQuestionChoices { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<ChannelOutput> ChannelOutputs { get; set; }
        public virtual DbSet<WowGuildAssociations> WowGuildAssociations { get; set; }
        public virtual DbSet<Giphy> Giphy { get; set; }
        public virtual DbSet<ServerSetting> ServerSettings { get; set; }
        public virtual DbSet<TriviaCategory> TriviaCategories { get; set; }
        public virtual DbSet<AwaySystem> AwaySystem { get; set; }
        public virtual DbSet<C8Ball> C8Ball { get; set; }
        public virtual DbSet<WowAuctions> WowAuctions { get; set; }
        public virtual DbSet<AuctionItemMapping> AuctionItemMappings { get; set; }
        public virtual DbSet<WowAuctionPrice> WowAuctionPrices { get; set; }
        public virtual DbSet<Blacklist> Blacklist { get; set; }
        public virtual DbSet<ServerGreeting> ServerGreetings { get; set; }
        public virtual DbSet<AchCategory> AchCategories { get; set; }
        public virtual DbSet<FindWowCheeve> FindWowCheeves { get; set; }
        public virtual DbSet<DiscordServer> DiscordServers { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<WowResources> WowResources { get; set; }
        public virtual DbSet<LogMonitoring> LogMonitoring { get; set; }
        public virtual DbSet<Warnings> Warnings { get; set; }
        public virtual DbSet<PrefixList> PrefixList { get; set; }
        public virtual DbSet<CharStats> CharStats { get; set; }
        public virtual DbSet<CurrentRaidTier> CurrentRaidTier { get; set; }
        public virtual DbSet<WowMChar> WowMChar { get; set; }
        public virtual DbSet<WordList> WordList { get; set; }
        public virtual DbSet<WowClassicGuild> WowClassicGuild { get; set; }
        public virtual DbSet<Randos> Randos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
            _config = _builder.Build();

            //var connectionStringBuilder = new  SqliteConnectionStringBuilder { DataSource = ".\\ninjabot.db" };
            //var connectionString = connectionStringBuilder.ToString();
            //var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlServer(_config["sqlConnectionString"]);// UseSqlite(connection);
            
        }
    }
}