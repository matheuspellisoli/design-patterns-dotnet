using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursos.Alura
{
    class Program
    {
        static void Main(string[] args)
        {


          var list = new List<Item>();
            list.Add(new Item("prod 1", 50.0));
            list.Add(new Item("prod 2", 60.5));

            list.Add(new Item("prod 4", 500.5));
            var orcamento = new Orcamento(list);

          var calculadorDeImposto = new  CalculadorDeImposto();

           calculadorDeImposto.Calcula(orcamento, new ImpostoIKCV());
        }


    }
    public class Item
    {
        public String Nome { get; private set; }
        public double Valor { get; private set; }

       public Item(String nome, double valor)
        {
            this.Nome = nome;
            this.Valor = valor;
        }


    }
    public class Orcamento
    {
       
        public List<Item> Itens { get; set; }
        public double Valor { get { return GetValor(); }}

        public Orcamento()
        {
            
        }
        public Orcamento(List<Item> itens)
        {
            this.Itens = itens;
        }

        public Double GetValor()
        {
            Double valor = 0;
            Itens.ForEach(item => { valor += item.Valor; });

            return valor;
        }

        public bool VerificaValor(double valor)
        {
            bool isValid = false;
            Itens.ForEach(item => { if (item.Valor > 100) { isValid = true; } });

            return isValid;
        }

       
    }

    public interface Imposto
    {
        double Calcula(Orcamento orcamento);
    }

    public abstract class TemplateDeImpostoCondicional : Imposto
    {
        public double Calcula(Orcamento orcamento)
        {
            if (DeveUsarMaximaTaxacao(orcamento))
            {
                return MaximaTaxacao(orcamento);
            }
            else
            {
                return MinimaTaxacao(orcamento);
            }
        }

        public abstract bool DeveUsarMaximaTaxacao(Orcamento orcamento);
        public abstract double MaximaTaxacao(Orcamento orcamento);
        public abstract double MinimaTaxacao(Orcamento orcamento);
    }
    

    public class CalculadorDeImposto
    {
        public void Calcula(Orcamento orcamento, Imposto estrategiaDeImposto)
        {
            double resultado = estrategiaDeImposto.Calcula(orcamento);
            Console.WriteLine(resultado);
        }
    }


    public class ImpostoICPP : TemplateDeImpostoCondicional
    {
        public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor > 500;
        }

        public override double MaximaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.07);
        }

        public override double MinimaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.05);
        }
    }

    public class ImpostoIKCV : TemplateDeImpostoCondicional
    {
        public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor > 500 && orcamento.VerificaValor(100);
        }

        public override double MaximaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.07);
        }

        public override double MinimaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.05);
        }
    }

    public class ImpostoIHIT : TemplateDeImpostoCondicional
    {
        public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
        {

            List<String> novalista = new List<string>();

            foreach(Item item in orcamento.Itens){

                if (novalista.Contains(item.Nome))
                    return true;
                novalista.Add(item.Nome);
            }

            return false ;
        }

       

        public override double MaximaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.07);
        }

        public override double MinimaTaxacao(Orcamento orcamento)
        {
            return orcamento.Valor + (orcamento.Valor * 0.05);
        }
    }
}
