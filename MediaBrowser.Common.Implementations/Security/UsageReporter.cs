﻿using MediaBrowser.Common.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediaBrowser.Common.Implementations.Security
{
    public class UsageReporter
    {
        private readonly IApplicationHost _applicationHost;
        private readonly INetworkManager _networkManager;
        private readonly IHttpClient _httpClient;

        public UsageReporter(IApplicationHost applicationHost, INetworkManager networkManager, IHttpClient httpClient)
        {
            _applicationHost = applicationHost;
            _networkManager = networkManager;
            _httpClient = httpClient;
        }

        public Task ReportServerUsage(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var mac = _networkManager.GetMacAddress();

            var data = new Dictionary<string, string>
            {
                { "feature", _applicationHost.Name }, 
                { "mac", mac }, 
                { "ver", _applicationHost.ApplicationVersion.ToString() }, 
                { "platform", Environment.OSVersion.VersionString }, 
                { "isservice", _applicationHost.IsRunningAsService.ToString().ToLower()}
            };

            return _httpClient.Post(Constants.Constants.MbAdminUrl + "service/registration/ping", data, cancellationToken);
        }

        public Task ReportAppUsage(ClientInfo app, CancellationToken cancellationToken)
        {
            // TODO: Implement this
            // Make sure to handle DeviceVersion being 

            return Task.FromResult(true);
        }
    }

    public class ClientInfo
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string DeviceVersion { get; set; }
    }
}
