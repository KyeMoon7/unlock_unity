using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;



public class OscProcess : MonoBehaviour
{
    public OSCReceiver oscReceiver;
    public OSCTransmitter oscTransmitter;
    public Unlock unlock;


    public int betaInMin = 0;
    public int betaInMax = 1023;
    public float betaOutMin = 0.0f;
    public float betaOutMax = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        oscReceiver.Bind("/tourner", TraiterMessagePot);
        oscReceiver.Bind("/valider", TraiterMessageBut);
    }

    // Update is called once per frame
    void Update()
    {
        if (unlock.isUnlocked())
        {
            var oscMessage = new OSCMessage("/lumiere");
            oscMessage.AddValue(OSCValue.Int(1)); // Ajoute l’entier 1
            oscTransmitter.Send(oscMessage);// Envoyer le message 
        }
        else
        {
            var oscMessage = new OSCMessage("/lumiere");
            oscMessage.AddValue(OSCValue.Int(0)); // Ajoute l’entier 0
            oscTransmitter.Send(oscMessage); // Envoyer le message 
        }


    }

    void TraiterMessagePot(OSCMessage message)
    {
        if (message.Values.Count != 1)
        {
            Debug.Log("Le message " + message.Address + " n’a pas le bon nombre d’arguments");
            return; // Quitte la fonction sans exécuter la suite
        }

        // Vérifiez que l’argument est du type attendu (`int` dans l’exemple) :
        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Le premier argument du message " + message.Address + "n’est pas un entier");
            return; // Quitte la fonction sans exécuter la suite
        }

        int valeur = message.Values[0].IntValue;
        float ajustee = (
            ((float)valeur - betaInMin) / (betaInMax - betaInMin) * (betaOutMax - betaOutMin) + betaOutMin
        );


        unlock.SetAngle(ajustee);


    }

    void TraiterMessageBut(OSCMessage message)
    {
        // Traiter un entier booléen (évènement discret)

        // Récupérer la valeur du premier argument en taant qu’entier :
        int valeur = message.Values[0].IntValue;

        // Deboguer
        // Debug.Log("Reçu : " + message.Address + " " + valeur);

        // Traiter la valeur
        if (valeur == 1)
        {
            // Si nécéssaire, appeler une fonction ici
            unlock.CheckSolution();
        }
        else
        {

        }

    }

}