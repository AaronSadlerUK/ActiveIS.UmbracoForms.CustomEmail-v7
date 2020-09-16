using System;
using System.Collections.Generic;
using System.IO;
using ActiveIS.UmbracoForms.CustomEmailV7.Interfaces;
using ActiveIS.UmbracoForms.CustomEmailV7.Services;
using Umbraco.Core.Logging;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;

namespace ActiveIS.UmbracoForms.CustomEmailV7.Workflows
{
    public class CustomEmailWorkflow : WorkflowType
    {
        [Setting("To Email", description = "Enter the receiver email address (Seperate multiple with a comma or semi-colon)", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string ToEmail { get; set; }

        [Setting("From Email", description = "Enter the sender email address", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string FromEmail { get; set; }

        [Setting("From Name", description = "Enter the sender name", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string FromName { get; set; }

        [Setting("Reply-to Email", description = "Enter the reply-to email address (Seperate multiple with a comma or semi-colon)", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string ReplyTo { get; set; }

        [Setting("Bcc Email", description = "Enter the bcc email address (Seperate multiple with a comma or semi-colon)", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string Bcc { get; set; }

        [Setting("Cc Email", description = "Enter the cc email address (Seperate multiple with a comma or semi-colon)", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string Cc { get; set; }

        [Setting("Email Heading", description = "Enter a heading for the email address", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string Heading { get; set; }

        [Setting("Subject", description = "Email subject", view = "~/App_Plugins/UmbracoForms/Backoffice/Common/SettingTypes/textfield.html")]
        public string Subject { get; set; }

        [Setting("Message", description = "Enter the intro message", view = "~/App_Plugins/Mw.UmbForms.Rte/editor.html")]
        public string Message { get; set; }

        [Setting("Template Name", description = "The path to the template that you want to use for generating the email. Email templates are stored at /views/partials/forms/customemails", view = "~/App_Plugins/ActiveIS.UmbracoForms.CustomEmail/Backoffice/Common/SettingTypes/customemailtemplatepicker.html")]
        public string TemplateName { get; set; }

        public CustomEmailWorkflow()
        {
            this.Id = new Guid("1e106db8-685d-441f-9c19-c5e344163c2c");
            this.Name = "Send Custom Email";
            this.Description = "This workflow is is to be used to send a custom templated email";
            this.Icon = "icon-message";
            this.Group = nameof(ToEmail);
        }

        public override List<Exception> ValidateSettings()
        {
            List<Exception> exceptionList = new List<Exception>();
            if (string.IsNullOrWhiteSpace(this.ToEmail))
                exceptionList.Add((Exception)new ArgumentNullException("ToEmail", "'To email is required"));
            if (string.IsNullOrWhiteSpace(this.Subject))
                exceptionList.Add((Exception)new ArgumentNullException("Subject", "'Subject is required'"));
            if (string.IsNullOrWhiteSpace(this.Message))
                exceptionList.Add((Exception)new ArgumentNullException("Message", "'Message is required'"));
            if (string.IsNullOrWhiteSpace(this.FromEmail))
                exceptionList.Add((Exception)new ArgumentNullException("FromEmail", "'From email is required'"));
            if (string.IsNullOrWhiteSpace(this.TemplateName))
                exceptionList.Add((Exception)new ArgumentNullException("TemplateName", "'TemplateName' setting has not been set'"));
            return exceptionList;
        }

        public override WorkflowExecutionStatus Execute(
          Record record,
          RecordEventArgs e)
        {
            try
            {
                var smtpService = new SmtpService();
                var template = $"~/Views/Partials/{TemplateName}";

                var emailBody = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(template))
                    .Replace("[[SUBJECT]]", Subject)
                    .Replace("[[HEADING]]", Heading)
                    .Replace("[[BODY]]", Message);

                smtpService.SendEmail(emailBody, ToEmail, FromEmail, FromName, Subject, ReplyTo, Bcc, Cc);
                return WorkflowExecutionStatus.Completed;
            }
            catch (Exception ex)
            {
                LogHelper.Error<CustomEmailWorkflow>(ex.Message, ex);
                return WorkflowExecutionStatus.Failed;
            }
        }
    }
}
