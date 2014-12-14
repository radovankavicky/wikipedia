﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WikiParser
{
    /// <summary>
    /// WindowsForm class
    /// </summary>
    public partial class Wiki : Form
    {
        private string rawInput = "vstup.xml";
        private string parsedInput = "vystup3.xml";
        private List<DisambiguationPageInfo> disambiguationPages;
        private Parser parser;

        /// <summary>
        /// Constructor
        /// </summary>
        public Wiki()
        {
            InitializeComponent();
            TbRawInput.Text = rawInput;
            TbParsedInput.Text = parsedInput;
            parser = new Parser();
        }

        /// <summary>
        /// Export stored disambiguation pages in XML file
        /// </summary>
        /// <param name="path"></param>
        private void ExportDisambiguationPages(string path)
        {
            using (StreamWriter sw = new StreamWriter(@path, false, Encoding.UTF8))
            {
                sw.WriteLine("<dspages>");
                foreach (DisambiguationPageInfo disambiguationPageInfo in disambiguationPages)
                {
                    sw.WriteLine(disambiguationPageInfo.ExportToString());
                }
                sw.WriteLine("</dspages>");
            }
        }  

        /// <summary>
        /// Click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseRawInput_Click(object sender, EventArgs e)
        {
            if (OfdRawInput.ShowDialog() == DialogResult.OK)
            {
                rawInput = OfdRawInput.FileName;
                TbRawInput.Text = rawInput;
            }
        }

        /// <summary>
        /// Click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnParse_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            disambiguationPages = parser.GetDisambiguationPagesFromWikiDump(rawInput);
            ShowDisambPages();
            DateTime end = DateTime.Now;
            RtbProgramInfo.Text += "Elapsed time: " + String.Format("{0:g}", (end - start)) + Environment.NewLine;
            RtbProgramInfo.Text += "Disambiguation pages: " + disambiguationPages.Count + Environment.NewLine;

            int childPageCount = 0;
            foreach (DisambiguationPageInfo disambPage in disambiguationPages)
            {
                childPageCount += disambPage.pages.Count;
            }
            RtbProgramInfo.Text += "Child pages: " + childPageCount;
        }

        /// <summary>
        /// Click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                RtbProgramInfo.Text += ("Start vypisu " + DateTime.Now + Environment.NewLine);
                ExportDisambiguationPages(saveFileDialog1.FileName);
                RtbProgramInfo.Text += ("Koniec vypisu " + DateTime.Now + Environment.NewLine);
            }

            
        }

        /// <summary>
        /// Show stored disambiguation pages and child pages
        /// </summary>
        private void ShowDisambPages()
        {
            RtbProgramInfo.Text = "";
            LbDisambPages.Items.Clear();
            LbPages.Items.Clear();
            LbDisambPages.Items.AddRange(disambiguationPages.ToArray());
            
            LbDisambPages.SelectedIndex = 0;
        }

        /// <summary>
        /// SelectedIndexChanged listBox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbDisambPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            LbPages.Items.Clear();
            LbPages.Items.AddRange(((DisambiguationPageInfo)LbDisambPages.SelectedItem).pages.ToArray());
            if(LbPages.Items.Count > 0)
                LbPages.SelectedIndex = 0;
        }

        /// <summary>
        /// SelectedIndexChanged listBox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            RtbPagesInfo.Clear();
            RtbPagesInfo.Text += ((PageInfo) (LbPages.SelectedItem)).ExportTo();
        }

        /// <summary>
        /// Click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            disambiguationPages = parser.GetDisambiguationPagesFromParsedWikiDump(parsedInput);
            ShowDisambPages();
            DateTime end = DateTime.Now;
            RtbProgramInfo.Text += "Elapsed time: " + String.Format("{0:g}", (end - start)) + Environment.NewLine;
            RtbProgramInfo.Text += "Disambiguation pages: " + disambiguationPages.Count + Environment.NewLine;

            int childPageCount = 0;
            foreach (DisambiguationPageInfo disambPage in disambiguationPages)
            {
                childPageCount += disambPage.pages.Count;
            }
            RtbProgramInfo.Text += "Child pages: " + childPageCount;
        }

        /// <summary>
        /// Click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowseParsedInput_Click(object sender, EventArgs e)
        {
            OfdParsedInput.InitialDirectory = @"C:\Users\Pleto\Documents\Visual Studio 2013\Projects\WikiParser\WikiParser\bin\Debug\";
            if (OfdParsedInput.ShowDialog() == DialogResult.OK)
            {
                parsedInput = OfdParsedInput.FileName;
                TbParsedInput.Text = parsedInput;
            }
        }
    }
}
