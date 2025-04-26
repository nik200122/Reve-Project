using System.Collections.Generic;
using UnityEngine;

public static class ActionFactory
{
    public static INPCAction CreateAction(string actionType, List<ParameterConfig> parameters)
    {
        // Puoi creare un dizionario per facilitare il lookup dei parametri, se necessario.
        Dictionary<string, string> paramDict = new Dictionary<string, string>();
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                paramDict[param.key] = param.value;
            }
        }

        switch (actionType)
        {
            case "WarnOthers":
                return new WarnOthersAction(paramDict);
            case "Talk":
                return new TalkAction(paramDict);
            case "Flee":
                return new FleeAction(paramDict);
            case "Ignore":
                return new IgnoreAction(paramDict);
            case "BeCautious":
                return new BeCautiousAction(paramDict);
            case "Approach":  // Nuovo caso per l'azione di approach
                return new ApproachAction(paramDict);
            case "Greet":  // Nuovo caso per l'azione di approach
                return new GreetAction(paramDict);
            case "GiveItem":
                return new GiveItemAction(paramDict);
                
            // Aggiungi altri casi a seconda delle azioni
            default:
                Debug.LogWarning("Azione non riconosciuta: " + actionType);
                return null;
        }
    }
}
