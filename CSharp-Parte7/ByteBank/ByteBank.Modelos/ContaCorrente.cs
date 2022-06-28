﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBank.Modelos
{
    /// <summary>
    /// Define uma Conta Corrente do banco ByteBank.
    /// </summary>
    public class ContaCorrente
    {
        private static int TaxaOperacao;
        public static int TotalDeContasCriadas { get; private set; }
        public Cliente Titular { get; set; }

        public int ContadorSaquesNaoPermitidos { get; private set; }
        public int ContadorTransferenciasNaoPermitidas { get; private set; }

        public int Numero { get; }
        public int Agencia { get; }

        private double _saldo = 100;
        public double Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                if (value < 0)
                {
                    return;
                }

                _saldo = value;
            }
        }

        /// <summary>
        /// Cria uma instância de ContaCorrente com os argumentos utilizados
        /// </summary>
        /// <param name="agencia"> Representa o valor da propriedade <see cref="Agencia"/> e deve possuir um valor maior que zero.</param>
        /// <param name="numero"> Representa o valor da propriedade <see cref="Numero"/> e deve possuir um valor maior que zero.</param>
        /// <exception cref="ArgumentException"></exception>
        public ContaCorrente(int agencia, int numero)
        {

            if (agencia <= 0)
            {
                throw new ArgumentException("O argumento agencia deve ser maior que zero.", nameof(agencia));
            }
            if (numero <= 0)
            {
                throw new ArgumentException("O argumento numero deve ser maior que zero.", nameof(numero));
            }
            

            Agencia = agencia;
            Numero = numero;

            TotalDeContasCriadas++;

            TaxaOperacao = 30 / TotalDeContasCriadas;
        }

        /// <summary>
        /// Realiza o saque e atualiza o valor da propriedade <see cref="Saldo"/>
        /// </summary>
        /// <param name="valor">Representa o valor do saque, deve ser maior que zero e menor que o <see cref="Saldo"/></param>
        /// <exception cref="ArgumentException">Exceção lançada quando um valor negativo é utilizado no argumento <paramref name="valor"/>.</exception>
        /// <exception cref="SaldoInsuficienteException">Exceção lançada quando o valor de <paramref name="valor"/> é maior que o valor da propriedade <see cref="Saldo"/>.</exception>
        public void Sacar(double valor)
        {
            if (valor < 0)
            {
                throw new ArgumentException("Valor inválido para o saque." + nameof(valor));
            }

            if (_saldo < valor)
            {
                ContadorSaquesNaoPermitidos++;
                throw new SaldoInsuficienteException(Saldo, valor);
            }

            _saldo -= valor;
        }

        public void Depositar(double valor)
        {
            _saldo += valor;
        }

        public void Transferir(double valor, ContaCorrente contaDestino)
        {
            if (valor < 0)
            {
                throw new ArgumentException("Valor inválido para a transferência." + nameof(valor));
            }

            try
            {
                Sacar(valor);
            }
            catch (SaldoInsuficienteException ex)
            {
                ContadorTransferenciasNaoPermitidas++;
                throw new OperacaoFinanceiraException("Operação não  realizada", ex);
            }

            contaDestino.Depositar(valor);

        }


        public override string ToString()
        {
            return $"Número: {Numero}, Agência: {Agencia}, Saldo: {Saldo}";            
            // return "Número: " + Numero + ", Agência: " + Agencia + ", Saldo: " + Saldo; 
        }

        public override bool Equals(object obj)
        {
            ContaCorrente outraConta = obj as ContaCorrente;

            if(outraConta == null)
            {
                return false;
            }

            //Este código abaixo pode ser escrito como o último return
            //if(Numero == outraConta.Numero && Agencia == outraConta.Agencia)
            //{
            //    return true;
            //}
            //return false;

            return Numero == outraConta.Numero && Agencia == outraConta.Agencia;

        }
    }
}
    
