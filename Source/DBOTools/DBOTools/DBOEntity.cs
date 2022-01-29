using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOTools
{
    /// Entities

   
    public class DBOType
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DBONamespace DBONamespace { get; set; }
    }

    public class DBOInstance
    {
        DBOType EntityType { get; set; }
        public DBONamespace DBONamespace { get; set; }
    }

    // Entity Types

    public enum DBONamespace
    {
        HVAC,
        LIGHTING,
        METERS,
        PHYSICAL_SECURITY
    }

    public class EntityType
    {
        public string Name { get; }
        public DBONamespace DBONamespace { get; }

        public EntityType(string name, DBONamespace dboNamespace)
        {
            Name = name;
            DBONamespace = DBONamespace;
        }

        public static EntityType AHU = new EntityType("AHU", DBONamespace.HVAC);
        public static EntityType ANALYSIS = new EntityType("ANALYSIS", DBONamespace.HVAC);
        public static EntityType BLR = new EntityType("BLR", DBONamespace.HVAC);
        public static EntityType CDWS = new EntityType("CDWS", DBONamespace.HVAC);
        public static EntityType CH = new EntityType("CH", DBONamespace.HVAC);
        public static EntityType CHWS = new EntityType("CHWS", DBONamespace.HVAC);
        public static EntityType CT = new EntityType("CT", DBONamespace.HVAC);
        public static EntityType DC = new EntityType("DC", DBONamespace.HVAC);
        public static EntityType DFR = new EntityType("DFR", DBONamespace.HVAC);
        public static EntityType DH = new EntityType("DH", DBONamespace.HVAC);
        public static EntityType DMP = new EntityType("DMP", DBONamespace.HVAC);
        public static EntityType FAN = new EntityType("FAN", DBONamespace.HVAC);
        public static EntityType FCU = new EntityType("FCU", DBONamespace.HVAC);
        public static EntityType HUM = new EntityType("HUM", DBONamespace.HVAC);
        public static EntityType WHS = new EntityType("WHS", DBONamespace.HVAC);
        public static EntityType HX = new EntityType("HX", DBONamespace.HVAC);
        public static EntityType MAU = new EntityType("MAU", DBONamespace.HVAC);
        public static EntityType PMP = new EntityType("PMP", DBONamespace.HVAC);
        public static EntityType SDC = new EntityType("SDC", DBONamespace.HVAC);
        public static EntityType UH = new EntityType("UH", DBONamespace.HVAC);
        public static EntityType VAV = new EntityType("VAV", DBONamespace.HVAC);
        public static EntityType WEATHER = new EntityType("WEATHER", DBONamespace.HVAC);
        public static EntityType WEBCTRL = new EntityType("WEBCTRL", DBONamespace.HVAC);
        public static EntityType ZONE = new EntityType("ZONE", DBONamespace.HVAC);

        public static readonly List<EntityType> EntityTypes = new List<EntityType>
        {
            AHU,
            ANALYSIS,
            BLR,
            CDWS,
            CH,
            CHWS,
            CT,
            DC,
            DFR,
            DH,
            DMP,
            FAN,
            FCU,
            HUM,
            WHS,
            HX,
            MAU,
            PMP,
            SDC,
            UH,
            VAV,
            WEATHER,
            WEBCTRL,
            ZONE,
        };

    }
    
}
