XPO-Tools
=========

Tools regarding XPO-file based import and export related to Dynamics AX.
Build for work with Dynamics AX 2009, but will also be available for Dynamics AX 2012.

Milestones and goals
====================

MS1 - The basics
----------------

* Let open any xpo file and display its content as done in Dynamics AX AOT. Display just the name and raw data of each element, not its evaluation.
* Provide functionality which lets user select one or more elements and export those to a new xpo file which contains only the selected elements.
* Labels in xpo files are not yet supported.

MS2 - Multiple xpo and a bit evaluation
---------------------------------------

* Lets open additional xpo files and merges into current list of elements.
* Supports evaluation for macros, classes and jobs.
* Therefore, supports syntax highlighting of X++ code.

MS3 - Multi-Export and table evaluation
---------------------------------------

* Provide functionality to export all selected elements into one file per element, maintaining a file system structure similar to it's AOT path.
* Evaluates table elements to display content.

MS4 - Even more evaluation and labels
-------------------------------------

* Support content evaluation for further (all interesting) element types.
* Support labels in xpo files.

MS5 - Suitable for Dynamics AX 2012
-----------------------------------

* Upgrade to work also with XPO-files produced by Dynamics AX 2012.
* Introduce new element types, used by Dynamics AX 2012.

Environment
===========

IDE used is Microsoft Visual Studio Express 2013 for Desktop.
Programming language is C#.
