# Revit Add-In: RevitConduitTable

## Introduction
This Revit Add-In extends the functionality of Autodesk Revit by adding a custom interface and integrating with external libraries. Admin privileges are required for proper operation.

## Installation
Before using the add-in, make sure to follow these steps:

1. Open the project in Visual Studio as an Administrator to ensure you have sufficient permissions to execute Post-Build events.

2. Install the necessary NuGet packages via the NuGet Package Manager or the NuGet Console using the commands provided in the project documentation.

3. Add References to Third-Party Revit libraries by navigating to `Project > Add Reference` in Visual Studio and selecting the appropriate `.dll` files required for the add-in.

## Post-Build Events
The project contains Post-Build events to copy necessary files to the correct locations for the add-in to function in Revit. These scripts are executed automatically after a successful build.

## Running the Add-In
To run the add-in within Revit:

1. Start Revit as an Administrator.
2. Navigate to the Add-Ins tab on the Revit ribbon.
3. Locate and execute RevitConduitTable.

## Troubleshooting
If you encounter issues with the add-in not appearing in Revit, verify that the Post-Build events have successfully copied the add-in files to the expected directories. Additionally, check that all NuGet packages and Third-Party libraries are correctly referenced in the project.

