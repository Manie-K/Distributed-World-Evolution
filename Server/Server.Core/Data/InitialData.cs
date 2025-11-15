namespace Server.Core.Data
{
    internal static class InitialData
    {
        public static ModuleDBEntity[] GetInitialData() => new ModuleDBEntity[] 
        {
            new ModuleDBEntity(true, "Red Plant", 1, 1, 10, 80, 80, EntityTypeEnum.Animal, 0, new int[] {101, 212, 307, 405})
            {
                ID = -1
            },
            new ModuleDBEntity(true, "Blue Plant", 2, 2, 6, 70, 70, EntityTypeEnum.Animal, 1, new int[] {101, 206, 305, 405})
            {
                ID = -2
            },
            new ModuleDBEntity(true, "Purple Plant", 5, 4, 4, 70, 70, EntityTypeEnum.Animal, 6, new int[] {105, 201, 307, 405})
            {
                ID = -3
            },
            new ModuleDBEntity(true, "Pig", 4, 4, 6, 80, 60, EntityTypeEnum.Animal, 2, new int[] {103, 208, 308, 403})
            {
                ID = -4
            },
            new ModuleDBEntity(true, "Boar", 7, 7, 6, 90, 70, EntityTypeEnum.Animal, 3, new int[] {101, 210, 301, 402})
            {
                ID = -5
            },
            new ModuleDBEntity(true, "White rabbit", 2, 3, 10, 50, 40, EntityTypeEnum.Animal, 4, new int[] {103, 207, 305, 402})
            {
                ID = -6
            },
            new ModuleDBEntity(true, "Brown rabbit", 2, 2, 9, 50, 40, EntityTypeEnum.Animal, 5, new int[] {103, 207, 303, 402})
            {
                ID = -7
            },
            new ModuleDBEntity(true, "Green slime", 1, 1, 8, 90, 50, EntityTypeEnum.Animal, 7, new int[] {103, 212, 304, 403})
            {
                ID = -8
            },
            new ModuleDBEntity(true, "Blue slime", 3, 2, 6, 90, 60, EntityTypeEnum.Animal, 8, new int[] {101, 205, 306, 402})
            {
                ID = -9
            },
            new ModuleDBEntity(true, "Red slime", 7, 5, 3, 80, 50, EntityTypeEnum.Animal, 9, new int[] {105, 204, 301, 406})
            {
                ID = -10
            },
            new ModuleDBEntity(true, "Green orc", 7, 7, 5, 90, 90, EntityTypeEnum.Animal, 10, new int[] {101, 201, 301, 405})
            {
                ID = -11
            },
            new ModuleDBEntity(true, "Blue orc", 5, 8, 5, 80, 90, EntityTypeEnum.Animal, 11, new int[] {105, 205, 301, 405})
            {
                ID = -12
            },
            new ModuleDBEntity(true, "Darkgreen orc", 10, 6, 5, 80, 90, EntityTypeEnum.Animal, 12, new int[] {105, 201, 307, 404})
            {
                ID = -13
            },
            new ModuleDBEntity(true, "Vampire", 7, 6, 3, 90, 90, EntityTypeEnum.Animal, 13, new int[] {102, 203, 302, 405})
            {
                ID = -14
            },
            new ModuleDBEntity(true, "Blue vampire", 5, 8, 5, 80, 90, EntityTypeEnum.Animal, 14, new int[] {102, 209, 302, 404})
            {
                ID = -15
            },
            new ModuleDBEntity(true, "Red vampire", 9, 6, 2, 80, 90, EntityTypeEnum.Animal, 15, new int[] {102, 202, 302, 403})
            {
                ID = -16
            },



            new ModuleDBEntity(true, "Human", 0, 0, 0, 100, 100, EntityTypeEnum.Human, 16, new int[] {})
            {
                ID = -17
            },



            new ModuleDBEntity(true, "Cosmo", 0, 0, 7, 10, 30, EntityTypeEnum.Plant, 17, new int[] {104, 213, 309, 401})
            {
                ID = -18
            },
            new ModuleDBEntity(true, "Daffodil", 0, 0, 7, 10, 30, EntityTypeEnum.Plant, 18, new int[] {104, 213, 309, 401})
            {
                ID = -19
            },
            new ModuleDBEntity(true, "Daisy", 0, 0, 6, 10, 40, EntityTypeEnum.Plant, 19, new int[] {104, 213, 309, 401})
            {
                ID = -20
            },
            new ModuleDBEntity(true, "Lavender", 0, 0, 5, 20, 60, EntityTypeEnum.Plant, 20, new int[] {104, 213, 309, 401})
            {
                ID = -21
            },
            new ModuleDBEntity(true, "Lily", 0, 0, 7, 10, 50, EntityTypeEnum.Plant, 21, new int[] {104, 213, 309, 401})
            {
                ID = -22
            },
            new ModuleDBEntity(true, "LilyOfTheValley", 0, 0, 8, 20, 60, EntityTypeEnum.Plant, 22, new int[] {104, 213, 309, 401})
            {
                ID = -23
            },
            new ModuleDBEntity(true, "Orchid", 0, 0, 4, 30, 70, EntityTypeEnum.Plant, 23, new int[] {104, 213, 309, 401})
            {
                ID = -24
            },
            new ModuleDBEntity(true, "Pansy", 0, 0, 10, 10, 50, EntityTypeEnum.Plant, 24, new int[] {104, 213, 309, 401})
            {
                ID = -25
            },
            new ModuleDBEntity(true, "Poppy", 0, 0, 8, 10, 70, EntityTypeEnum.Plant, 25, new int[] {104, 213, 309, 401})
            {
                ID = -26
            },
            new ModuleDBEntity(true, "Rose", 0, 0, 4, 30, 100, EntityTypeEnum.Plant, 26, new int[] {104, 213, 309, 401})
            {
                ID = -27
            },
            new ModuleDBEntity(true, "Sunflower", 0, 0, 6, 40, 80, EntityTypeEnum.Plant, 27, new int[] {104, 213, 309, 401})
            {
                ID = -28
            },
            new ModuleDBEntity(true, "Tulip", 0, 0, 5, 20, 100, EntityTypeEnum.Plant, 28, new int[] {104, 213, 309, 401})
            {
                ID = -29
            }
        };
    }
}
