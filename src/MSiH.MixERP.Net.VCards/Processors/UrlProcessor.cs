using System;
using System.Collections.Generic;
using System.Text;
using MSiH.MixERP.Net.VCards;
using MSiH.MixERP.Net.VCards.Models;
using MSiH.MixERP.Net.VCards.Serializer;
using MSiH.MixERP.Net.VCards.Types;

namespace MSiH.MixERP.Net.VCards.Processors
{
    public static class UrlProcessor
    {
        public static string Serialize(VCard vcard)
        {
            if (vcard.Url == null)
            {
                return string.Empty;
            }

            string url = vcard.Url.ToString();

            return DefaultSerializer.GetVCardString("URL", url, true, vcard.Version);
        }

        public static string SerializeUris(IEnumerable<Uri> uris, string key, VCardVersion version)
        {
            var builder = new StringBuilder();
            if (uris == null)
            {
                return string.Empty;
            }

            int preference = 0;

            foreach (var uri in uris)
            {
                if (uri == null)
                {
                    continue;
                }

                preference++;

                string memberKey = key;

                memberKey = memberKey + ";PREF=" + preference;

                builder.Append(DefaultSerializer.GetVCardString(memberKey, uri.ToString(), false, version));
            }

            return builder.ToString();
        }

        public static void Parse(Token token, ref VCard vcard)
        {
            string url = token.Values[0];
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                try
                {
                    vcard.Url = new Uri(url, UriKind.RelativeOrAbsolute);
                    // Use the uri
                }
                catch (UriFormatException ex)
                {
                    Console.WriteLine("UriFormatException: " + ex.Message);
                }
            }
        }
    }
}
