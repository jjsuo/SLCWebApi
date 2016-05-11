/********************************************************************************
** Copyright(c) 2016  All Rights Reserved. 
** auth：索俊杰
** mail：suojunjie@hotmail.com
** date： 2016/5/10 0:07:11 
** desc：  
** Ver : V1.0.0 
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace Common
{
    public class _CrmService
    {


        public static OrganizationServiceProxy OrgService()
        {
            string userName = ConfigurationManager.AppSettings["AdminUserName"];
            string password = ConfigurationManager.AppSettings["AdminUserPassword"];
            string domainName = ConfigurationManager.AppSettings["AdminUserDomainName"];
            string organizationUri = @"http://" + ConfigurationManager.AppSettings["IP"] + "/" + ConfigurationManager.AppSettings["OrgName"] + "/XRMServices/2011/Organization.svc";
            string discoveryUri = @"http://" + ConfigurationManager.AppSettings["IP"] + "/" + ConfigurationManager.AppSettings["OrgName"] + "/XRMServices/2011/Discovery.svc";
            //to ignore certificates errors
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertificatePolicy;

            IServiceConfiguration<IDiscoveryService> discoveryConfiguration = ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(new Uri(discoveryUri));
            AuthenticationProviderType endpointType = discoveryConfiguration.AuthenticationType;
            OrganizationServiceProxy organizationServiceProxy = null;
            if (endpointType == AuthenticationProviderType.ActiveDirectory)
            {
                //AD          
                Uri OrganizationUri = new Uri(organizationUri);
                ClientCredentials credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(userName, password, domainName);
                organizationServiceProxy = new OrganizationServiceProxy(OrganizationUri, null, credentials, null);
            }
            else if (endpointType == AuthenticationProviderType.Federation)
            {
                //IFD
                ClientCredentials userCredentials = new ClientCredentials();
                userCredentials.UserName.UserName = domainName + "\\" + userName; ;
                userCredentials.UserName.Password = password;

                SecurityTokenResponse userResponseWrapper = discoveryConfiguration.Authenticate(userCredentials);
                var _discServiceProxy = new DiscoveryServiceProxy(discoveryConfiguration, userResponseWrapper);
                IServiceConfiguration<IOrganizationService> serviceConfiguration = ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(new Uri(organizationUri));
                organizationServiceProxy = new OrganizationServiceProxy(serviceConfiguration, userResponseWrapper);
                organizationServiceProxy.EnableProxyTypes();
            }

            return organizationServiceProxy;
        }
        private static bool AcceptAllCertificatePolicy(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        // CRM系统服务
        public static OrganizationServiceProxy OrgService(Guid userId)
        {
            string userName = ConfigurationManager.AppSettings["AdminUserName"];
            string password = ConfigurationManager.AppSettings["AdminUserPassword"];
            string domainName = ConfigurationManager.AppSettings["AdminUserDomainName"];
            string organizationUri = @"http://" + ConfigurationManager.AppSettings["IP"] + "/" + ConfigurationManager.AppSettings["OrgName"] + "/XRMServices/2011/Organization.svc";
            string discoveryUri = @"http://" + ConfigurationManager.AppSettings["IP"] + "/" + ConfigurationManager.AppSettings["OrgName"] + "/XRMServices/2011/Discovery.svc";

            //to ignore certificates errors
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertificatePolicy;

            IServiceConfiguration<IDiscoveryService> discoveryConfiguration = ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(new Uri(discoveryUri));
            AuthenticationProviderType endpointType = discoveryConfiguration.AuthenticationType;

            OrganizationServiceProxy organizationServiceProxy = null;
            if (endpointType == AuthenticationProviderType.ActiveDirectory)
            {
                //AD          
                Uri OrganizationUri = new Uri(organizationUri);
                ClientCredentials credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(userName, password, domainName);
                organizationServiceProxy = new OrganizationServiceProxy(OrganizationUri, null, credentials, null);
                organizationServiceProxy.CallerId = userId;
            }
            else if (endpointType == AuthenticationProviderType.Federation)
            {
                //IFD
                ClientCredentials userCredentials = new ClientCredentials();
                userCredentials.UserName.UserName = domainName + "\\" + userName; ;
                userCredentials.UserName.Password = password;

                SecurityTokenResponse userResponseWrapper = discoveryConfiguration.Authenticate(userCredentials);
                var _discServiceProxy = new DiscoveryServiceProxy(discoveryConfiguration, userResponseWrapper);
                IServiceConfiguration<IOrganizationService> serviceConfiguration = ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(new Uri(organizationUri));
                organizationServiceProxy = new OrganizationServiceProxy(serviceConfiguration, userResponseWrapper);
                organizationServiceProxy.EnableProxyTypes();
                organizationServiceProxy.CallerId = userId;
            }

            return organizationServiceProxy;
        }
    }


    public static class CrmService
    {
        private static OrganizationServiceProxy _orgService;

        public static OrganizationServiceProxy OrgService
        {
            get
            {
                if (_orgService == null)
                {
                    _orgService = _CrmService.OrgService();
                    return _orgService;
                }
                else
                {
                    return _orgService;
                }

            }
        }
    }
}
