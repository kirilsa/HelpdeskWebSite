using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace HelpDeskWebSite.Models
{
    public class EmailMessage
    {
        [Key]
        public int Id { get; set; }

        [ModelBinder(Name = "recipient")]
        public string? Recipient { get; set; } = string.Empty;

        [ModelBinder(Name = "sender")]
        public string? Sender { get; set; } = string.Empty;

        [ModelBinder(Name = "from")]
        public string? From { get; set; } = string.Empty;

        [ModelBinder(Name = "subject")]
        public string? Subject { get; set; } = string.Empty;

        [ModelBinder(Name = "body-plain")]
        public string? BodyPlain { get; set; } = string.Empty;

        [ModelBinder(Name = "stripped-text")]
        public string? StrippedText { get; set; } = string.Empty;

        [ModelBinder(Name = "stripped-signature")]
        public string? StrippedSignature { get; set; } = string.Empty;

        [ModelBinder(Name = "body-html")]
        public string? BodyHtml { get; set; } = string.Empty;

        [ModelBinder(Name = "stripped-html")]
        public string? StrippedHtml { get; set; } = string.Empty;

        [ModelBinder(Name = "attachment-count")]
        public int? AttachmentCount { get; set; } = null;

        [ModelBinder(Name = "timestamp")]
        public int? Timestamp { get; set; } = null;

        [ModelBinder(Name = "token")]
        public string? Token { get; set; } = string.Empty;

        [ModelBinder(Name = "signature")]
        public string? Signature { get; set; } = string.Empty;

        [ModelBinder(Name = "message-headers")]
        public string? MessageHeaders { get; set; } = string.Empty;

        [ModelBinder(Name = "content-id-map")]
        public string? ContentIdMap { get; set; } = string.Empty;

        [ModelBinder(Name = "Date")]
        public DateTimeOffset Date { get; set; }
        public int? InReply { get; set; } = null;
        public string? Status { get; set; } = string.Empty;
        public string? EmailType { get; set; } = string.Empty; // received, sent, note

        [BindNever]
        public virtual ListOfRequests? listOfRequests { get; set; }
    }
}
