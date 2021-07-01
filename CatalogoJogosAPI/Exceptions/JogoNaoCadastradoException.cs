using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogosAPI.Exceptions
{
    public class JogoNaoCadastradoException : Exception
    {
        public JogoNaoCadastradoException()
            : base("Este jogo já está cadastrado") 
        { }
    }
}
