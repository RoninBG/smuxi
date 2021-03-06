/*
 * $Id$
 * $URL$
 * $Rev$
 * $Author$
 * $Date$
 *
 * smuxi - Smart MUltipleXed Irc
 *
 * Copyright (c) 2005-2006 Mirco Bauer <meebey@meebey.net>
 *
 * Full GPL License: <http://www.gnu.org/licenses/gpl.txt>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 */

using System;
using System.IO;
using System.Reflection;
using Smuxi.Common;

namespace Smuxi.Server
{
    public class MainClass
    {
#if LOG4NET
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif
        
        public static void Main(string[] args)
        {
            bool debug = false;
            foreach (string arg in args) {
                switch (arg) {
                    case "-d":
                    case "--debug":
                        debug = true;
                        break;
                    case "-h":
                    case "--help":
                        ShowHelp();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option: " + arg);
                        Environment.Exit(1);
                        break;
                }
            }
            
#if LOG4NET
            // initialize log level
            log4net.Repository.ILoggerRepository repo = log4net.LogManager.GetRepository();
            if (debug) {
                repo.Threshold = log4net.Core.Level.Debug;
            } else {
                repo.Threshold = log4net.Core.Level.Info;
            }
#endif

            string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string localeDir = Path.Combine(appDir, "locale");
            if (!Directory.Exists(localeDir)) {
                localeDir = Path.Combine(Defines.InstallPrefix, "share");
                localeDir = Path.Combine(localeDir, "locale");
            }

            LibraryCatalog.Init("smuxi-server", localeDir);
#if LOG4NET
            _Logger.Debug("Using locale data from: " + localeDir);
#endif
            try {
                Server.Init(args);
            } catch (Exception e) {
#if LOG4NET
                _Logger.Fatal(e);
#endif
                // rethrow the exception for console output
                throw;
            }
        }
        
        private static void ShowHelp()
        {
            Console.WriteLine("Usage: smuxi-server [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -h --help   Show this help");
            Console.WriteLine("  -d --debug  Enable debug output");
        }
    }
}
