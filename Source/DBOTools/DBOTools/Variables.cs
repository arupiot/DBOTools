﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOTools
{
    public class Variables
    {
        //public static List<string> ExcludedCategories = new List<string>
        //{
        //"Cable Tray Fittings",
        //"Cable Tray Tags",
        //"Callout Heads",
        //"Conduit Fittings",
        //"Conduit Tags",
        //"Data Device Tags",
        //"Detail Item Tags",
        //"Detail Items",
        //"Duct Accessory Tags",
        //"Duct Fittings",
        //"Duct Tags",
        //"Electrical Equipment Tags",
        //"Electrical Fixture Tags",
        //"Elevation Marks",
        //"Generic Annotations",
        //"Generic Model Tags",
        //"Generic Models",
        //"Grid Heads",
        //"Level Heads",
        //"Material Tags",
        //"Mechanical Equipment Tags",
        //"Pipe Accessory Tags",
        //"Pipe Fittings",
        //"Plumbing Fixture Tags",
        //"Rebar Shape",
        //"Revision Cloud Tags",
        //"Room Tags",
        //"Section Marks",
        //"Space Tags",
        //"Spot Elevation Symbols",
        //"Structural Framing",
        //"Telephone Device Tags",
        //"Title Blocks",
        //"View Reference",
        //"View Titles",
        //"Wire Tags",
        //};


        List<string> AllCategories = new List<string>
        {
        "1535_00_KRO_2_TW_G_O4_0106_A_20200122.dwg",
        "1535_00_KRO_2_TW_G_O5_0107_A_20200122.dwg",
        "1535_00_KRO_2_TW_G_U1_0101_A_20200122.dwg",
        "Adaptive Points",
        "Air Systems",
        "Air Terminal Tags",
        "Air Terminals",
        "Analysis Display Style",
        "Analysis Results",
        "Analytical Beam Tags",
        "Analytical Beams",
        "Analytical Brace Tags",
        "Analytical Braces",
        "Analytical Column Tags",
        "Analytical Columns",
        "Analytical Floor Tags",
        "Analytical Floors",
        "Analytical Foundation Slabs",
        "Analytical Isolated Foundation Tags",
        "Analytical Isolated Foundations",
        "Analytical Link Tags",
        "Analytical Links",
        "Analytical Node Tags",
        "Analytical Nodes",
        "Analytical Pipe Connections",
        "Analytical Slab Foundation Tags",
        "Analytical Spaces",
        "Analytical Surfaces",
        "Analytical Wall Foundation Tags",
        "Analytical Wall Foundations",
        "Analytical Wall Tags",
        "Analytical Walls",
        "Anchor Tags",
        "Annotation Crop Boundary",
        "Area Load Tags",
        "Area Tags",
        "Areas",
        "Assemblies",
        "Assembly Tags",
        "Bolt Tags",
        "Boundary Conditions",
        "Brace in Plan View Symbols",
        "Bridge Abutments",
        "Bridge Arches",
        "Bridge Bearings",
        "Bridge Cables",
        "Bridge Decks",
        "Bridge Foundations",
        "Bridge Girders",
        "Bridge Piers",
        "Bridge Towers",
        "Cable Tray Fitting Tags",
        "Cable Tray Fittings",
        "Cable Tray Runs",
        "Cable Tray Tags",
        "Cable Trays",
        "Callout Boundary",
        "Callout Heads",
        "Callouts",
        "Cameras",
        "Casework",
        "Casework Tags",
        "Ceiling Tags",
        "Ceilings",
        "Color Fill Legends",
        "Columns",
        "Communication Device Tags",
        "Communication Devices",
        "Conduit Fitting Tags",
        "Conduit Fittings",
        "Conduit Runs",
        "Conduit Tags",
        "Conduits",
        "Connection Symbols",
        "Contour Labels",
        "Coordination Model",
        "Crop Boundaries",
        "Curtain Grids",
        "Curtain Panel Tags",
        "Curtain Panels",
        "Curtain System Tags",
        "Curtain Systems",
        "Curtain Wall Mullions",
        "Data Device Tags",
        "Data Devices",
        "Detail Item Tags",
        "Detail Items",
        "Dimensions",
        "Displacement Path",
        "Door Tags",
        "Doors",
        "Duct Accessories",
        "Duct Accessory Tags",
        "Duct Color Fill",
        "Duct Color Fill Legends",
        "Duct Fitting Tags",
        "Duct Fittings",
        "Duct Insulation Tags",
        "Duct Insulations",
        "Duct Lining Tags",
        "Duct Linings",
        "Duct Placeholders",
        "Duct Systems",
        "Duct Tags",
        "Ducts",
        "Electrical Circuits",
        "Electrical Equipment",
        "Electrical Equipment Tags",
        "Electrical Fixture Tags",
        "Electrical Fixtures",
        "Electrical Spare/Space Circuits",
        "Elevation Marks",
        "Elevations",
        "Entourage",
        "Filled region",
        "Fire Alarm Device Tags",
        "Fire Alarm Devices",
        "Flex Duct Tags",
        "Flex Ducts",
        "Flex Pipe Tags",
        "Flex Pipes",
        "Floor Tags",
        "Floors",
        "Foundation Span Direction Symbol",
        "Furniture",
        "Furniture System Tags",
        "Furniture Systems",
        "Furniture Tags",
        "Generic Annotations",
        "Generic Model Tags",
        "Generic Models",
        "Grid Heads",
        "Grids",
        "Guide Grid",
        "HLSE-UG-475_clean.dwg",
        "HVAC Zones",
        "H_4210_clean.dwg",
        "H_4280_clean.dwg",
        "Hole Tags",
        "Imports in Families",
        "Internal Area Load Tags",
        "Internal Line Load Tags",
        "Internal Point Load Tags",
        "Keynote Tags",
        "Level Heads",
        "Levels",
        "Lighting Device Tags",
        "Lighting Devices",
        "Lighting Fixture Tags",
        "Lighting Fixtures",
        "Line Load Tags",
        "Lines",
        "MEP Fabrication Containment",
        "MEP Fabrication Containment Tags",
        "MEP Fabrication Ductwork",
        "MEP Fabrication Ductwork Tags",
        "MEP Fabrication Hanger Tags",
        "MEP Fabrication Hangers",
        "MEP Fabrication Pipework",
        "MEP Fabrication Pipework Tags",
        "Masking Region",
        "Mass",
        "Mass Floor Tags",
        "Mass Tags",
        "Matchline",
        "Material Tags",
        "Materials",
        "Mechanical Equipment",
        "Mechanical Equipment Set Boundary Lines",
        "Mechanical Equipment Set Tags",
        "Mechanical Equipment Sets",
        "Mechanical Equipment Tags",
        "Model Groups",
        "Multi-Category Tags",
        "Multi-Rebar Annotations",
        "Nurse Call Device Tags",
        "Nurse Call Devices",
        "Panel Schedule Graphics",
        "Parking",
        "Parking Tags",
        "Part Tags",
        "Parts",
        "Path of Travel Tags",
        "Pipe Accessories",
        "Pipe Accessory Tags",
        "Pipe Color Fill",
        "Pipe Color Fill Legends",
        "Pipe Fitting Tags",
        "Pipe Fittings",
        "Pipe Insulation Tags",
        "Pipe Insulations",
        "Pipe Placeholders",
        "Pipe Segments",
        "Pipe Tags",
        "Pipes",
        "Piping Systems",
        "Plan Region",
        "Planting",
        "Planting Tags",
        "Plate Tags",
        "Plumbing Fixture Tags",
        "Plumbing Fixtures",
        "Point Clouds",
        "Point Load Tags",
        "Profile Tags",
        "Project Information",
        "Property Line Segment Tags",
        "Property Tags",
        "RVT Links",
        "Railing Tags",
        "Railings",
        "Ramps",
        "Raster Images",
        "Rebar Cover References",
        "Rebar Set Toggle",
        "Rebar Shape",
        "Reference Lines",
        "Reference Planes",
        "Reference Points",
        "Render Regions",
        "Revision Cloud Tags",
        "Revision Clouds",
        "Roads",
        "Roof Tags",
        "Roofs",
        "Room Tags",
        "Rooms",
        "Routing Preferences",
        "Schedule Graphics",
        "Schedules",
        "Scope Boxes",
        "Section Boxes",
        "Section Line",
        "Section Marks",
        "Sections",
        "Security Device Tags",
        "Security Devices",
        "Shaft Openings",
        "Shear Stud Tags",
        "Sheets",
        "Site",
        "Site Tags",
        "Space Tags",
        "Spaces",
        "Span Direction Symbol",
        "Specialty Equipment",
        "Specialty Equipment Tags",
        "Spot Coordinates",
        "Spot Elevation Symbols",
        "Spot Elevations",
        "Spot Slopes",
        "Sprinkler Tags",
        "Sprinklers",
        "Stair Landing Tags",
        "Stair Paths",
        "Stair Run Tags",
        "Stair Support Tags",
        "Stair Tags",
        "Stair Tread/Riser Numbers",
        "Stairs",
        "Structural Annotations",
        "Structural Area Reinforcement",
        "Structural Area Reinforcement Symbols",
        "Structural Area Reinforcement Tags",
        "Structural Beam System Tags",
        "Structural Beam Systems",
        "Structural Column Tags",
        "Structural Columns",
        "Structural Connection Tags",
        "Structural Connections",
        "Structural Fabric Areas",
        "Structural Fabric Reinforcement",
        "Structural Fabric Reinforcement Symbols",
        "Structural Fabric Reinforcement Tags",
        "Structural Foundation Tags",
        "Structural Foundations",
        "Structural Framing",
        "Structural Framing Tags",
        "Structural Internal Loads",
        "Structural Load Cases",
        "Structural Loads",
        "Structural Path Reinforcement",
        "Structural Path Reinforcement Symbols",
        "Structural Path Reinforcement Tags",
        "Structural Rebar",
        "Structural Rebar Coupler Tags",
        "Structural Rebar Couplers",
        "Structural Rebar Tags",
        "Structural Stiffener Tags",
        "Structural Stiffeners",
        "Structural Truss Tags",
        "Structural Trusses",
        "Switch System",
        "System-Zone Tags",
        "System-Zones",
        "Telephone Device Tags",
        "Telephone Devices",
        "Text Notes",
        "Title Blocks",
        "Topography",
        "View Reference",
        "View Titles",
        "Viewports",
        "Views",
        "Wall Tags",
        "Walls",
        "Water Loops",
        "Weld Tags",
        "Window Tags",
        "Windows",
        "Wire Tags",
        "Wires",
        "Zone Equipment",
        "Zone Tags",
        "h_4240_clean.dwg",
        "h_4250_clean.dwg",
        "h_4260_clean.dwg",
        "h_4270_clean.dwg",
        "hl_4220_clean.dwg",
        "hl_4230_clean.dwg",
        "l_501_KG_clear.dwg",
        "l_507_5OG.dwg",
        };


        public static List<string> IncludedCategories = new List<string>
        {
        "Air Terminals",
        "Assemblies",
        "Communication Devices",
        "Conduits",
        "Data Devices",
        "Doors",
        "Ducts",
        "Electrical Equipment",
        "Electrical Fixtures",
        "Fire Alarm Devices",
        "Furniture",
        "Generic Models",
        "Lighting Devices",
        "Lighting Fixtures",
        "Mechanical Equipment",
        "Model Groups",
        "Nurse Call Devices",
        "Security Devices",
        "Specialty Equipment",
        "Sprinklers",
        "Telephone Devices",
        "Windows",
        "Zone Equipment",
        };
    }
}
