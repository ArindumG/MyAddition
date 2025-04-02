using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MyAddition
{
    public class MyFingerJoint : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyFingerJoint class.
        /// </summary>
        public MyFingerJoint()
          : base("MyFingerJoint", "Finger",
              "GeneratesFingerJoints",
              "MyPlugin", "Joinery")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Edge", "E", "Base edge for the finger", GH_ParamAccess.item);
            pManager.AddNumberParameter("FingerWidth", "W", "Width for the finger", GH_ParamAccess.item);
            pManager.AddNumberParameter("OffsetDistance", "D", "Offset for the finger", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Finger joint", "FD", "Base edge for the finger", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Line edge = new Line();
            double fingerWidth = 0;
            double offsetDistance = 0;

            if (!DA.GetData(0, ref edge)) return;
            if (!DA.GetData(1, ref fingerWidth)) return;
            if (!DA.GetData(2, ref offsetDistance)) return;

            List<Curve> connectors;
            List<Curve> fingers = GenerateFingerJoints(edge, fingerWidth, offsetDistance, out connectors);

            List<Curve> result = new List<Curve>();
            result.AddRange(fingers);
            result.AddRange(connectors);

            DA.SetDataList(0, result);

        }

        private List<Curve> GenerateFingerJoints(Line edge, double fingerWidth, double offsetDistance, out List<Curve> connectors)
        {
            List<Curve> fingers = new List<Curve>();
            connectors = new List<Curve>();
            double length = edge.Length;

            int fingerCount = (int)(length / fingerWidth);
            double actualFingerWidth = length / fingerCount; // Adjust finger width to fit exactly

            // Calculate a perpendicular direction to the edge
            Vector3d edgeDirection = edge.Direction;
            Vector3d perpendicularDirection = Vector3d.CrossProduct(edgeDirection, Vector3d.ZAxis);
            perpendicularDirection.Unitize();
            perpendicularDirection *= offsetDistance; // Scale by the given offset distance

            List<Point3d> topPoints = new List<Point3d>();
            List<Point3d> bottomPoints = new List<Point3d>();

            for (int i = 0; i < fingerCount; i++)
            {
                // Calculate the start and end points of each finger segment
                double t1 = i * actualFingerWidth / length;
                double t2 = (i + 1) * actualFingerWidth / length;

                Point3d p1 = edge.PointAt(t1);  // Start point of the finger
                Point3d p2 = edge.PointAt(t2);  // End point of the finger

                Line finger = new Line(p1, p2); // Create the finger as a line segment

                // Move alternate fingers to create projections and notches
                if (i % 2 == 0)  // Even index -> projection
                {
                    finger.Transform(Transform.Translation(perpendicularDirection));
                }
                else  // Odd index -> notch (move in opposite direction)
                {
                    finger.Transform(Transform.Translation(-perpendicularDirection));
                }

                fingers.Add(finger.ToNurbsCurve());

                // Store top and bottom points for connecting lines
                topPoints.Add(finger.From);
                bottomPoints.Add(finger.To);
            }
            topPoints.RemoveAt(0);
            bottomPoints.RemoveAt(bottomPoints.Count - 1);

            // Create perpendicular connecting lines between top and bottom points
            for (int i = 0; i < topPoints.Count; i++)
            {
                Line connector = new Line(topPoints[i], bottomPoints[i]); // Perpendicular connector
                connectors.Add(connector.ToNurbsCurve());
            }

            return fingers;

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
            get { return new Guid("9DBCDA1B-0EFF-4902-86D5-2591D9A5EB40"); }
        }
    }
}