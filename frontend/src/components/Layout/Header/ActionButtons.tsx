import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "../../ui/sheet";
import { AlignJustify } from "lucide-react";
import { Button } from "../../ui/button";
import { Link } from "react-router-dom";
import Logo from "./Logo";
import { useAuth } from "../../../context/useAuth";
type Props = {};

const ActionButtons = (props: Props) => {
  const { isLoggedIn } = useAuth();
  const { logout } = useAuth();
  return (
    <div>
      <div className="md:hidden">
        <Sheet>
          <SheetTrigger>
            <AlignJustify />
          </SheetTrigger>
          <SheetContent>
            <SheetHeader>
              <SheetDescription>
                <div className="flex flex-col space-y-4 items-start w-full text-lg text-black">
                  <Logo />
                  <Link to="/login">Sign in</Link>
                  <Link to="/">Get Started</Link>
                  <Link to="/">Pricing</Link>
                  <Link to="/">Contact</Link>
                  <Link to="/">About</Link>
                </div>
              </SheetDescription>
            </SheetHeader>
          </SheetContent>
        </Sheet>
      </div>

      <div className="hidden md:flex md:space-x-2">
        {isLoggedIn() ? (
          <>
            <Button className="text-md" onClick={logout}>
              Logout
            </Button>
          </>
        ) : (
          <>
            <Link to="/login">
              <Button className="text-md" variant="ghost">
                Sign in
              </Button>
            </Link>
            <Link to="/register">
              <Button className="text-md">Get Started</Button>
            </Link>
          </>
        )}
      </div>
    </div>
  );
};

export default ActionButtons;
