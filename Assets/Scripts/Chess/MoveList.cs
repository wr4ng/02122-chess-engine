using System.Collections.Generic;

namespace Chess
{
    public class Element
    {
        public Element next = null;
        public Move move;
        public Element(Move move, Element next)
        {
            this.move = move;
            this.next = next;
        }
    }
    public class MoveList
    {
        Element checkHead, checkTail, captureHead, captureTail, restHead = null;
        Element head = new Element(new(), null);
        Element prevSearched;
        public int prevIndex = 0;
        public int Count;
        public MoveList()
        {
            Insert(new());
            Count = 0;
            prevSearched = new Element(new(), head);
        }

        private (Element, Element) Insert(Move move, Element head, Element tail)
        {
            Element e = new(move, head);
            if (tail == null) tail = e;
            head = e;
            Count++;
            return (head, tail);
        }
        public void InsertCheck(Move move)
        {
            (checkHead, checkTail) = Insert(move, checkHead, checkTail);
        }
        public void InsertCapture(Move move)
        {
            (captureHead, captureTail) = Insert(move, captureHead, captureTail);
        }

        public void Insert(Move move)
        {
            Element e = new(move, restHead);
            restHead = e;
            Count++;
        }

        public void ConnectList()
        {
            //Connecting the list
            if (checkTail != null) checkTail.next = captureHead ?? restHead;
            if (captureTail != null) captureTail.next = restHead;
            //Setting the head
            head = new Element(new(), checkHead ?? captureHead ?? restHead);
            prevSearched.next = head.next;
        }

        public Move Get()
        {
            prevSearched = prevSearched.next;
            prevIndex++;
            return prevSearched.move;
        }
    }

    public class EasierMoveList
    {
        public List<Move> checks = new List<Move>();
        public List<Move> captures = new List<Move>(64);
        public List<Move> rest = new List<Move>(64);
        public List<Move> moves;

        public void InsertCheck(Move move)
        {
            checks.Add(move);
        }
        public void InsertCapture(Move move)
        {
            captures.Add(move);
        }
        public void Insert(Move move)
        {
            rest.Add(move);
        }
        public void ConnectList()
        {
            moves = new List<Move>(checks.Count + captures.Count + rest.Count);
            moves.AddRange(checks);
            moves.AddRange(captures);
            moves.AddRange(rest);
        }
        public List<Move> GetList(){
            return moves;
        }
    }

    public class MoveArray
    {
        Move[] checks = new Move[30];
        Move[] captures = new Move[32];
        Move[] rest = new Move[40];

        int checkCount, captureCount, restCount = 0;
        int checkIndex, captureIndex, restIndex = 0;

        public void InsertCheck(Move move)
        {
            if (checkCount == 30) return;
            checks[checkCount] = move;
            checkCount++;
        }

        public void InsertCapture(Move move)
        {
            if (captureCount == 32) return;
            captures[captureCount] = move;
            captureCount++;
        }

        public void Insert(Move move)
        {
            if (restCount == 40) return;
            rest[restCount] = move;
            restCount++;
        }

        public Move Get()
        {
            if (checkIndex < checkCount) { checkIndex++; return checks[checkIndex]; }
            if (captureIndex < captureCount) { captureIndex++; return captures[captureIndex]; }
            if (restIndex < restCount) { restIndex++; return rest[restIndex]; }
            return new();
        }
    }
    public class justaList{
        List<Move> moves = new();
        public void InsertCapture(Move move)
        {
            moves.Add(move);
        }
        public void Insert(Move move)
        {
            moves.Add(move);
        }
        public void ConnectList()
        {
            return;
        }
        public List<Move> GetList(){
            return moves;
        }
    }
}