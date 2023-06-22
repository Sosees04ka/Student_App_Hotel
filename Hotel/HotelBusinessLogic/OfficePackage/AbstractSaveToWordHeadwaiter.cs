using HotelBusinessLogic.OfficePackage.HelperEnums;
using HotelBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWordHeadwaitre
    {
        public void CreateDoc(WordInfoHeadwaiter info)
        {
            CreateWord(info);

            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties { Bold = true, Size = "24", }) },
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });

            foreach (var mc in info.DinnerRooms)
            {
                CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)>
                    { (mc.DinnerName, new WordTextProperties { Size = "24", Bold=true})},
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationType = WordJustificationType.Both
                    }
                });

                foreach (var mealPlan in mc.Rooms)
                {
                    CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)>
                        { (mealPlan.Item1 + " - ", new WordTextProperties { Size = "20", Bold=false}),
                        (mealPlan.Item2.ToString(), new WordTextProperties { Size = "20", Bold=false})},
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationType = WordJustificationType.Both
                        }
                    });
                }
            }
            SaveWord(info);
        }

        protected abstract void CreateWord(WordInfoHeadwaiter info);
        protected abstract void CreateParagraph(WordParagraph paragraph);
        protected abstract void SaveWord(WordInfoHeadwaiter info);
    }
}
