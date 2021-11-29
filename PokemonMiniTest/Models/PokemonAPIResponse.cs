using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonMiniTest.Models
{
    public class PokemonResponse
        {
            public int id { get; set; }
            public string name { get; set; }
            public int order { get; set; }
            public int gender_rate { get; set; }
            public int capture_rate { get; set; }
            public int base_happiness { get; set; }
            public bool is_baby { get; set; }
            public bool is_legendary { get; set; }
            public bool is_mythical { get; set; }
            public int hatch_counter { get; set; }
            public bool has_gender_differences { get; set; }
            public bool forms_switchable { get; set; }
            public Growth_Rate growth_rate { get; set; }
            public Pokedex_Numbers[] pokedex_numbers { get; set; }
            public Egg_Groups[] egg_groups { get; set; }
            public Color color { get; set; }
            public Shape shape { get; set; }
            public Evolves_From_Species evolves_from_species { get; set; }
            public Evolution_Chain evolution_chain { get; set; }
            public Habitat habitat { get; set; }
            public Generation generation { get; set; }
            public Name[] names { get; set; }
            public Flavor_Text_Entries[] flavor_text_entries { get; set; }
            public Form_Descriptions[] form_descriptions { get; set; }
            public Genera[] genera { get; set; }
            public Variety[] varieties { get; set; }

            protected bool Equals(PokemonResponse other)
            {
                return id == other.id && name == other.name && order == other.order && gender_rate == other.gender_rate && capture_rate == other.capture_rate && base_happiness == other.base_happiness && is_baby == other.is_baby && is_legendary == other.is_legendary && is_mythical == other.is_mythical && hatch_counter == other.hatch_counter && has_gender_differences == other.has_gender_differences && forms_switchable == other.forms_switchable && Equals(growth_rate, other.growth_rate) && Equals(pokedex_numbers, other.pokedex_numbers) && Equals(egg_groups, other.egg_groups) && Equals(color, other.color) && Equals(shape, other.shape) && Equals(evolves_from_species, other.evolves_from_species) && Equals(evolution_chain, other.evolution_chain) && Equals(habitat, other.habitat) && Equals(generation, other.generation) && Equals(names, other.names) && Equals(flavor_text_entries, other.flavor_text_entries) && Equals(form_descriptions, other.form_descriptions) && Equals(genera, other.genera) && Equals(varieties, other.varieties);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PokemonResponse) obj);
            }

            public override int GetHashCode()
            {
                var hashCode = new HashCode();
                hashCode.Add(id);
                hashCode.Add(name);
                hashCode.Add(order);
                hashCode.Add(gender_rate);
                hashCode.Add(capture_rate);
                hashCode.Add(base_happiness);
                hashCode.Add(is_baby);
                hashCode.Add(is_legendary);
                hashCode.Add(is_mythical);
                hashCode.Add(hatch_counter);
                hashCode.Add(has_gender_differences);
                hashCode.Add(forms_switchable);
                hashCode.Add(growth_rate);
                hashCode.Add(pokedex_numbers);
                hashCode.Add(egg_groups);
                hashCode.Add(color);
                hashCode.Add(shape);
                hashCode.Add(evolves_from_species);
                hashCode.Add(evolution_chain);
                hashCode.Add(habitat);
                hashCode.Add(generation);
                hashCode.Add(names);
                hashCode.Add(flavor_text_entries);
                hashCode.Add(form_descriptions);
                hashCode.Add(genera);
                hashCode.Add(varieties);
                return hashCode.ToHashCode();
            }
        }

        public class Habitat
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Growth_Rate
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Color
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Shape
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Evolves_From_Species
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Evolution_Chain
        {
            public string url { get; set; }
        }

        public class Generation
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Pokedex_Numbers
        {
            public int entry_number { get; set; }
            public Pokedex pokedex { get; set; }
        }

        public class Pokedex
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Egg_Groups
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Name
        {
            public string name { get; set; }
            public Language language { get; set; }
        }

        public class Language
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Flavor_Text_Entries
        {
            public string flavor_text { get; set; }
            public Language1 language { get; set; }
            public Version version { get; set; }
        }

        public class Language1
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Version
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Form_Descriptions
        {
            public string description { get; set; }
            public Language2 language { get; set; }
        }

        public class Language2
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Genera
        {
            public string genus { get; set; }
            public Language3 language { get; set; }
        }

        public class Language3
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Variety
        {
            public bool is_default { get; set; }
            public Pokemon pokemon { get; set; }
        }

        public class Pokemon
        {
            public string name { get; set; }
            public string url { get; set; }

        }


}
