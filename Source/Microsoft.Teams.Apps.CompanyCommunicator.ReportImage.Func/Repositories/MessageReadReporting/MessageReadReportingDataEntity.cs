// <copyright file="MessageReadReportingDataEntity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Sending notification entity class.
    /// This entity holds the information about a message being read, triggered by loading the reporting image for a notification
    /// </summary>
    public class MessageReadReportingDataEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the notification id.
        /// </summary>
        public string NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of the message read entry.
        /// </summary>
        public string Id { get; set; }

        public string DefaultLanguage { get; set; }
    }
}
