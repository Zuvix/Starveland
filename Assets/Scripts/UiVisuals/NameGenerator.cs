using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static readonly string DefaultName = "John Doe";
    private static readonly List<string> NamePool = new List<string>()
    {
        "Syrin",
        "Ptorik",
        "Revvyn",
        "Hodus",
        "Dimian",
        "Paskel",
        "Kontas",
        "Weston",
        "Azamar",
        "Jather",
        "Tekren",
        "Jareth",
        "Adon",
        "Zaden",
        "Eune",
        "Graff",
        "Tez",
        "Jessop",
        "Gunnar",
        "Pike",
        "Domnhar",
        "Baske",
        "Jerrick",
        "Mavrek",
        "Riordan",
        "Wulfe",
        "Straus",
        "Tyvrik",
        "Henndar",
        "Favroe",
        "Whit",
        "Jaris",
        "Renham",
        "Kagran",
        "Lassrin",
        "Vadim",
        "Arlo",
        "Quintis",
        "Vale",
        "Caelan",
        "Yorjan",
        "Khron",
        "Ishmael",
        "Jakrin",
        "Fangar",
        "Roux",
        "Baxar",
        "Hawke",
        "Gatlen",
        "Barak",
        "Nazim ",
        "Kadric",
        "Paquin",
        "Kent",
        "Moki",
        "Rankar",
        "Lothe",
        "Ryven",
        "Clawsen",
        "Pakker",
        "Embre",
        "Cassian",
        "Verssek",
        "Dagfinn",
        "Ebraheim",
        "Nesso",
        "Eldermar",
        "Rivik",
        "Rourke",
        "Barton",
        "Hemm",
        "Sarkin",
        "Blaiz",
        "Talon",
        "Agro",
        "Zagaroth",
        "Turrek",
        "Esdel",
        "Lustros",
        "Zenner",
        "Baashar",
        "Dagrod",
        "Gentar",
        "Feston"
    };
    public static string GenerateName()
    {
        if (NamePool.Count < 1)
        {
            return DefaultName;
        }
        string GeneratedName = NamePool[Random.Range(0, NamePool.Count)];
        NamePool.Remove(GeneratedName);
        return GeneratedName;
    }
}