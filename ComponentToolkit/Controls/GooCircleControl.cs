﻿using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentToolkit
{
    internal class GooCircleControl : GooVerticalControlBase<GH_Circle>
    {
        private Circle_Control type => (Circle_Control)Instances.Settings.GetValue(typeof(Circle_Control).FullName, 0);

        public override Guid AddCompnentGuid => type == Circle_Control.CNR ? new Guid("d114323a-e6ee-4164-946b-e4ca0ce15efa") :
            new Guid("807b86e3-be8d-4970-92b5-f8cdcb45b06b");

        public GooCircleControl(Func<GH_Circle> valueGetter, Func<bool> isNull, string name) : base(valueGetter, isNull, name)
        {

        }

        protected override GH_Circle SetValue(IGH_Goo[] values)
        {
            switch (type)
            {
                default:
                    return (GH_Circle)values[0];
                case Circle_Control.Plane_Radius:
                    return new GH_Circle(new Rhino.Geometry.Circle(((GH_Plane)values[0]).Value, ((GH_Number)values[1]).Value));
                case Circle_Control.CNR:
                    return new GH_Circle(new Rhino.Geometry.Circle(new Rhino.Geometry.Plane(((GH_Point)values[0]).Value, ((GH_Vector)values[1]).Value), 
                        ((GH_Number)values[2]).Value));
            }
        }

        protected override BaseControlItem[] SetControlItems()
        {
            switch (type)
            {
                default:
                    return new BaseControlItem[]
                    {
                        new GooInputBoxStringControl<GH_Circle>(()=> ShowValue, _isNull, true),
                    };
                case Circle_Control.Plane_Radius:
                    return new BaseControlItem[]
                    {
                         new GooPlaneControl(()=>
                         {
                             if(ShowValue == null) return null;
                             return new GH_Plane(ShowValue.Value.Plane);
                         }, _isNull, "P"),

                         new GooNumberControl(()=>
                         {
                             if(ShowValue == null) return null;
                             return new GH_Number(ShowValue.Value.Radius);
                         }, _isNull, "R"),
                    };
                case Circle_Control.CNR:
                    return new BaseControlItem[]
                    {
                         new GooPointControl(()=>
                         {
                             if(ShowValue == null) return null;
                             return new GH_Point(ShowValue.Value.Center);
                         }, _isNull, "C"),


                         new GooVectorControl(()=>
                         {
                             if(ShowValue == null) return null;
                             return new GH_Vector(ShowValue.Value.Normal);
                         }, _isNull, "N"),

                         new GooNumberControl(()=>
                         {
                             if(ShowValue == null) return null;
                             return new GH_Number(ShowValue.Value.Radius);
                         }, _isNull, "R"),
                    };
            }
        }

        public override void DosomethingWhenCreate(IGH_DocumentObject obj)
        {
            if (obj == null) return;
            GH_Component com = (GH_Component)obj;
            if (com == null) return;

            if (type == Circle_Control.CNR)
            {                
                if (com.Params.Input.Count < 3) return;

                if (com.Params.Input[0] is Param_Point)
                {
                    Param_Point param = (Param_Point)com.Params.Input[0];
                    GH_Point point = ((GooPointControl)_values[0])._savedValue;
                    if (point != null)
                    {
                        param.PersistentData.Clear();
                        param.PersistentData.Append(point);
                    }
                }

                if (com.Params.Input[1] is Param_Vector)
                {
                    Param_Vector param = (Param_Vector)com.Params.Input[1];
                    GH_Vector vector = ((GooVectorControl)_values[1])._savedValue;
                    if (vector != null)
                    {
                        param.PersistentData.Clear();
                        param.PersistentData.Append(vector);
                    }
                }

                if (com.Params.Input[2] is Param_Number)
                {
                    Param_Number param = (Param_Number)com.Params.Input[2];
                    GH_Number number = ((GooInputBoxStringControl<GH_Number>)_values[2])._savedValue;
                    if (number != null)
                    {
                        param.PersistentData.Clear();
                        param.PersistentData.Append(number);
                    }
                }
            }
            else
            {
                if (com.Params.Input.Count < 2) return;

                if (com.Params.Input[0] is Param_Plane)
                {
                    Param_Plane param = (Param_Plane)com.Params.Input[0];
                    GH_Plane plane = ((GooPlaneControl)_values[0])._savedValue;
                    if (plane != null)
                    {
                        param.PersistentData.Clear();
                        param.PersistentData.Append(plane);
                    }
                }

                if (com.Params.Input[1] is Param_Number)
                {
                    Param_Number param = (Param_Number)com.Params.Input[1];
                    GH_Number number = ((GooInputBoxStringControl<GH_Number>)_values[1])._savedValue;
                    if (number != null)
                    {
                        param.PersistentData.Clear();
                        param.PersistentData.Append(number);
                    }
                }
            }

        }
    }
}
