using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap_Max<T> where T : IHeapItem<T>
{
    T[] items;              //tableau qui va recevoir les sommets
    int currentItemCount;   //nombre d'item dans le tableau a un moment précis

    //Permet d'avoir la grosseur du tableau
    public Heap_Max(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    //Permet d'ajouter un item (sommet) au tableau
    public void Ajouter(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        TrierVersHaut(item);
        currentItemCount++;
    }

    //permet d'enlever un sommet et le remplace par le dernier sommet
    //(le dernier item du tableau
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        TrierVersBas(items[0]);
        return firstItem;
    }

    //Permet, si un item (sommet) a un nouveau fCost, de le replacer à la bonne postion
    //et le nouveau fCost ne peut être que plus petit puisque sa valeur maximale de l'item
    //sera changer seulment s'il a trouver un chemin plus rapide
    public void UpdateItems(T item)
    {
        TrierVersHaut(item);
    }


    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    //Permet de savoir si la liste contient l'item (le sommet) envoyé 
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    //Permet de placer le le nouvelle item (sommet) à la bonne position
    //dans l'arbre/tas (heap). Cet item est comparé avec ses deux enfants
    //et, s'il est plus grand, échanger de position avec le plus petit des deux
    void TrierVersBas(T item) {
		while (true)
        {
			int childIndexGauche = item.HeapIndex * 2 + 1;
			int childIndexDroite = item.HeapIndex * 2 + 2;
			int indexEchange = 0;

            //Vérfie si il y a un child « gauche », puisque le child est trouvé avec la formule plus haut
            //et le sommet pourrait ne pas avoir de child du tout
			if (childIndexGauche < currentItemCount)
            {
				indexEchange = childIndexGauche;

                //Vérfie s'il y a un child « droite », puisque le child est trouvé avec la formule plus haut
                //et le sommet pourrait seulment avoir un child gauche sans child droit (seulment 1 child au lieu de deux
                if (childIndexDroite < currentItemCount) {
					if (items[childIndexGauche].CompareTo(items[childIndexDroite]) < 0) 
                    indexEchange = childIndexDroite;
				}

				if (item.CompareTo(items[indexEchange]) < 0) 
				Echanger (item,items[indexEchange]);

                //si le parent est plus grand que ses deux enfants, il est dans sa bonne position et termine le trie
				else 
				return;
			}
            //si la parent n'a pas d'enfant, termine les échanges
			else
		    return;
		}
	}

    //Si la valeur de la case est plus petite que son parent,
    //Échange leur position dans le tableau
    void TrierVersHaut(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Echanger(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    //Échange la position entre deux sommet dans le tableau
    void Echanger(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int tempIndexItemA = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempIndexItemA;
    }
}

/*Aussi utilisé par Élian
 * 
//Permet à chaque items (sommet) de garder en mémoire
//leur indexe dans le tableau
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
*/