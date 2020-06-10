What is WUDownloader?  
6/9/2020 - v1.2.0.1

WUDownloader is a standalone console application designed to collect information on and download offline installers
for Windows Updates.

Requirements:  
--.NET Framework 4.0 or newer  
--Internet Connection  
--Windows Update component installed  
--User must have privileges to run Windows Update (Some organizations disable this privilege for end-users)  
--Depending on privileges, Admin privileges may be necessary for standard mode
--Windows 7, Windows 8, Windows 8.1, Windows 10, Windows Server 2008 (Untested), Windows Server 2012 (Untested), Windows Server 2012 R2 (Untested), Windows Server 2016, Windows Server 2019 (Untested)

Features:  
--WUDownloader supports both portable and standard mode.
--Any update that is found on the official Microsoft Update Catalog can be fetched.
--WUDownloader will automatically skip any updates that are not found in the Catalog.
--Supports scanning current OS for updates, or through file import by list of KB nmumbers.
--Supports downloading of any platform/product supported by a given update.
--Filters out updates utilizing any non-English languages. Additional language support planned.
--Displays download progress in real-time. Downloads are currently 1 at a time. Multiple downloads simultaneously planned.  
--Allows filtering of platforms to download for.
--Supports downloading both available updates and already installed updated.  

Known Bugs:  
--Language Packs *sometimes* crash WUDownloader. This is a GUID issue. Solution planned.  
--When scanning for updates, updates that already exist in the UpdateCatalog.csv sometimes are skipped for download. Solution planned.  
--Application will crash if UpdateCatalog.csv or Updates.txt are open when importing or exporting. Solution planned.  
--Internet connection drop can crash the application. Researching solution.  
--Application will crash if Windows Update component is removed from the OS.  
--When scanning for available updates and none are available, the product filtering will be empty.  

Plans for the Future:  
--Add a GUI option  
--Add command line switches for menu selections  
--Pause or skip downloads  
--View file size  
--Preview download queue prior to starting  
--Add architecture data (x86, x64, etc)  
--Automatically sort downloads by architecture  
--Automatically detect successor updates  
--Add filter for file types (.msu, .exe, etc.)  
--Add preferences to config.txt  
--Support languages other than English  
--Support .csv or .xlsx file types for file import feature  
--Multiple asynchronous downloads  
--Automatically detect missing prerequisites  
--Support restricting collection only to specific platform  
--Support drivers available through Windows Update  
--Create additional .csv that stores skipped update information collection  

To run WUDownloader in portable mode, simply create an empty .txt file named "portable.txt" and place it in 
the same directory as WUDownloader.exe. This will cause WUDownloader to create all necessary folders and files 
in that directory.  

Running WUDownloader in standard mode will create all folders and files at C:\WUDownloader

This is the file structure created by WUDownloader:

{Root Directory}\Config\config.txt  
{Root Directory}\Downloads  
{Root Directory}\Import  
{Root Directory}\Table  
{Root Directory}\Logs\log.txt  

Config - Holds the config.txt file, which contains file path configuration for WUDownloader.  
Downloads - All downloads will be saved here. The format is [ProductName]\[Update Title]\[Filename].[extension]
	Note: WUDownloader will automatically shrink filepaths to fit the 260 character limit. This means that
	if you move WUDownloader's root folder, you will be unable to utilize a filepath above 260 characters
	(Support for moving all folders and files to new location will be added later).  
Import - Import files go here. For now, only .txt files containing one update per line. This method will only scan
	for KB numbers, any anything else will be ignored.  
Table - UpdateCatalog.csv will be created and stored here once you begin collecting update information. This file
	contains all of the data that is collected by WUDownloader, and is required to download updates.  
Logs - Holds log.txt, a file that stores logs for WUDownloader.  

For anyone errors or questions, please contact trentfromrid@gmail.com  
If you encounter an error, please provide screenshots of the error.
