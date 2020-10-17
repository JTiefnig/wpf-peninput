using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Collections;

namespace PenInputCanvas
{




    public interface TransformationStack: IEnumerable<Matrix>
    {
        

    }



    public class MandalaTransformation: TransformationStack
    {



        public MandalaTransformation()
        {
        }


        public int Subdivisions { get; set; } = 1;


        public IEnumerator<Matrix> GetEnumerator()
        {
            
            for(int i=0; i< Subdivisions; i++)
            {

                yield return new Matrix();
            }


        }

        IEnumerator IEnumerable.GetEnumerator()
        {


            return GetEnumerator();
        }
    }




}
