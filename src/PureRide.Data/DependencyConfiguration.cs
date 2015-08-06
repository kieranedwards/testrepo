using System.Data.SqlClient;
using Autofac;
using DapperWrapper;
using PureRide.Data.Interfaces.Configuration;
using PureRide.Data.Repositories;

namespace PureRide.Data
{
    public class DependencyConfiguration : Module
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;

        public DependencyConfiguration(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(a =>
            {
                var sqlConnection = new SqlConnection(_databaseConnectionSettings.PureRideConnectionString);
                sqlConnection.Open();
                return new SqlExecutor(sqlConnection);
            }).As<IDbExecutor>();

            /*builder.Register(a => new SqlExecutorFactory(_databaseConnectionSettings.PureRideConnectionString))
                .As<IDbExecutorFactory>()
                .SingleInstance();*/
            
            builder.Register(a => new ClipCardRepository(a.Resolve<IDbExecutor>())).As<IClipCardRepository>().SingleInstance(); 
        }
    }
}