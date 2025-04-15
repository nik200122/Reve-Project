using System.Collections.Generic;
using UnityEngine;

public class BattleCalculatorManager : MonoBehaviour
{
    private DamageFormula damageFormula;

    public void SetDamageFormula(DamageFormula loadedDamageFormula){
        damageFormula = loadedDamageFormula;
    }

    public float EvaluateDamage(IHittable attacker, IHittable defender){
        List<FormulaToken> tokens = damageFormula.tokens;
        Stack<float> values = new();
        Stack<MathOperator> operators = new();

        Debug.Log("📌 Inizio valutazione formula");
        Debug.Log($"🔍 Numero di token nella formula: {tokens.Count}");

        foreach (var token in tokens){
            if (token is OperatorToken op){
                operators.Push(op.operation);
                Debug.Log($"🔢 Operatore aggiunto: {op.operation}");
            }else{
                float val = token.Evaluate(attacker, defender);
                Debug.Log($"📥 Valore valutato: {val} da token {token}");

                values.Push(val);

                if (values.Count >= 2 && operators.Count > 0){
                    float right = values.Pop();
                    float left = values.Pop();
                    var mathOp = operators.Pop();

                    float result = OperatorToken.Apply(left, right, mathOp);

                    Debug.Log($"⚙️ Operazione: {left} {mathOp} {right} = {result}");

                    values.Push(result);
                }
            }
        }

        float finalValue = values.Count > 0 ? values.Pop() : 0f;
        Debug.Log($"✅ Risultato finale formula: {finalValue}");
        return finalValue;
    }
}
