﻿using Microsoft.Extensions.Logging;
using Reporting.HtmlEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Reporting
{
    public sealed class HtmlJsCssCleanupEngine : IHtmlRenderEngine
    {
        private Dictionary<string, string> _linkedCssCache = new Dictionary<string, string>();
        private string _rootPath;
        private readonly ILogger _logger;

        public HtmlJsCssCleanupEngine(string rootPath, ILoggerFactory loggerFactory)
        {
            _rootPath = rootPath;
            _logger = loggerFactory.CreateLogger<HtmlJsCssCleanupEngine>();
        }
        public async Task<string> RenderAsync(object model, string htmlSource)
        {
            htmlSource = ResolveCSSLinks(htmlSource);
            var result = PreMailer.Net.PreMailer.MoveCssInline(htmlSource);
            return await Task.FromResult(result.Html);
        }

        private string ResolveCSSLinks(string html)
        {
            var matches = Regex.Matches(html, @"<link\s*\s+href=""([^""]+)""\s*/>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                try
                {
                    string css = string.Empty;
                    string link = match.Groups[1].Value;
                    if (!_linkedCssCache.ContainsKey(link))
                    {
                        if (link.Contains("http"))
                        {
                            var webRequest = HttpWebRequest.Create(link);
                            var webResponse = webRequest.GetResponse();

                            using (Stream stream = webResponse.GetResponseStream())
                            {
                                StreamReader sr = new StreamReader(stream);

                                css = RemovePremailerNetBrokenSelectorModifiers(sr.ReadToEnd());
                            }
                        }
                        else if(!string.IsNullOrEmpty(_rootPath))
                        {
                            var currentPath = _rootPath + link.Replace("../", "").Replace("/", "\\");
                            css = RemovePremailerNetBrokenSelectorModifiers(File.ReadAllText(currentPath));
                            
                        }

                        _linkedCssCache.Add(link, css);
                    }
                    else
                        css = _linkedCssCache[link];

                    html = html.Replace(match.Value,"").Replace("<body>", string.Format("<style type=\"text/css\">{0}</style><body>", css));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Css error : ", ex);
                }
            }
            return html;
        }
        private static string RemovePremailerNetBrokenSelectorModifiers(string css)
        {
            return Regex.Replace(css, @"[:]+-[\-a-z0-9]+", string.Empty, RegexOptions.IgnoreCase);
        }

    }
}
