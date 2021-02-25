import React, { useContext } from "react";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import DropdownButton from "../../molecules/DropdownButton";
import ProfileNavLink from "../../molecules/Links/ProfileNavLink";
import { SessionContext } from "../../../store/context/session-context";

const Root = styled.div`
  position: relative;
`;

const ListItem = styled.li`
  display: inline-block;
  margin: 0 ${(p) => p.theme.size.distance};

  a.actived {
    color: ${(p) => p.theme.color.darkText};
    text-decoration: none;
    font-weight: 600;
    border-bottom: 3px solid ${(p) => p.theme.color.primaryBg};
  }

  :hover a {
    color: ${(p) => p.theme.color.darkText};
    text-decoration: none;
  }

  a {
    color: ${(p) => p.theme.color.secondaryTitle};
    font-weight: 500;
    font-size: ${(p) => p.theme.fontSize.small};
    border: 0;
    height: ${(p) => p.theme.size.medium};
    line-height: ${(p) => p.theme.size.medium};
    display: inline-block;
    height: 100%;
  }
`;

const UserDropdown = styled(DropdownButton)`
  height: 100%;
  ul {
    min-width: 200px;
  }
  ul li > a:hover {
    background-color: ${(p) => p.theme.color.lightBg};
  }
  > button {
    height: 100%;
    padding: 10px 15px;
    line-height: 1;
  }
  > button,
  > button:hover,
  > button:focus {
    background-color: transparent;
    border: 0;
    color: ${(p) => p.theme.color.neutralText};
  }
`;

export default (function (props) {
  const { className, userId, baseUrl } = props;
  var { currentUser, isLogin } = useContext(SessionContext);
  const currentUserDropdown = [
    {
      url: `${baseUrl}/${userId}/update`,
      name: "Update My Information",
      isNav: true,
    },
    {
      url: `${baseUrl}/${userId}/security`,
      name: "Security",
      isNav: true,
    },
  ];

  const otherUserDropdown = [];

  const navs = [
    {
      pageNav: "",
      title: "Feeds",
    },
    {
      pageNav: "articles",
      title: "Articles",
    },
    {
      pageNav: "farms",
      title: "Farms",
    },
    {
      pageNav: "products",
      title: "Products",
    },
    {
      pageNav: "about",
      title: "About Me",
    },
    {
      pageNav: "followings",
      title: "Followings",
    },
  ];

  return (
    <Root>
      <div className="row">
        <div className="col-auto me-auto">
          <HorizontalList className={className}>
            {navs.map((nav) => {
              return (
                <ListItem key={nav.pageNav}>
                  <ProfileNavLink
                    pageNav={nav.pageNav}
                    {...props}
                    userId={userId}
                    baseUrl={baseUrl}
                  >
                    {nav.title}
                  </ProfileNavLink>
                </ListItem>
              );
            })}
          </HorizontalList>
        </div>

        <div className="col-auto">
          {isLogin && currentUser.userIdentityId === userId ? (
            <UserDropdown icon="ellipsis-v" dropdown={currentUserDropdown} />
          ) : isLogin ? (
            <UserDropdown icon="ellipsis-v" dropdown={otherUserDropdown} />
          ) : null}
        </div>
      </div>
    </Root>
  );
});
