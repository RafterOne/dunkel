dunkel
======

A set of helper classes and libraries to support Sitecore implementations.

This solution builds to a .dll that can be included in Sitecore projects.  It includes classes and methods that we commonly use in Sitecore implementations, including:

- Base classes for pages and controls (sublayouts)
- Helper methods
- Extensions

TO USE

- In the solution file, update the references to Sitecore.Kernel to use your copies of the libraries.
- Build the solution.
- Copy the resulting PixelMEDIA.SitecoreCMS.dll into your Sitecore solution.
- To use the item event handlers under PixelMEDIA.SitecoreCMS.Controls.EventHandlers, you will need to add references to the desired methods to your EventHandlers.config file (or your web.config).  See the comments in each class for more details.