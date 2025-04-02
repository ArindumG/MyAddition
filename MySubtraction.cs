using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MyAddition
{
    public class MySubtraction : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public MySubtraction()
          : base("Subtraction", "Subtract",
              "Calculates the difference of 2 numbers",
              "MyPlugin", "Utility")
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
            pManager.AddNumberParameter("Subtraction", "Subtract", "Calculates the difference", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a = double.NaN;
            double b = double.NaN;

            DA.GetData(0, ref a);
            DA.GetData(1, ref b);

            double subtract = a - b;

            DA.SetData(0, subtract);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2B8E979D-5EDF-4AB7-A2C4-7AF2D2C8F34A"); }
        }
    }
}