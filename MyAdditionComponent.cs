using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MyAddition
{
  public class MyAdditionComponent : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public MyAdditionComponent()
      : base("Addition", 
            "Add",
        "Calculates the Sum of 2 numbers",
        "MyPlugin", 
        "Utility")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
            pManager.AddNumberParameter("First Number", "First", "The first Number", GH_ParamAccess.item, 0.0);

            pManager.AddNumberParameter("Second Number", "Second", "The Second Number", GH_ParamAccess.item, 0.0);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
            pManager.AddNumberParameter("Addition", "Add", "Calculates the sum", GH_ParamAccess.item);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
            double a = double.NaN;
            double b = double.NaN;

            DA.GetData(0, ref a);
            DA.GetData(1, ref b);

            double add = a + b;

            DA.SetData(0, add);
        }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// You can add image files to your project resources and access them like this:
    /// return Resources.IconForThisComponent;
    /// </summary>
    protected override System.Drawing.Bitmap Icon => null;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("e5c30f80-8393-49f8-bfb1-b6f73d572e76");
  }
}